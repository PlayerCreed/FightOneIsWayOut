using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Textscript : MonoBehaviour
{
    public Transform arm;

    [Tooltip("Rotation speed of the fps controller."), SerializeField]
    private float mouseSensitivity = 7f;

    [Tooltip("The position of the arms and gun camera relative to the fps controller GameObject."), SerializeField]
    private Vector3 armPosition;

    [Tooltip("Approximately the amount of time it will take for the fps controller to reach maximum rotation speed."), SerializeField]
    private float rotationSmoothness = 0.05f;

    [Tooltip("Minimum rotation of the arms and camera on the x axis."),SerializeField]
    private float minVerticalAngle = -90f;

    [Tooltip("Maximum rotation of the arms and camera on the axis."),SerializeField]
    private float maxVerticalAngle = 90f;

    [Tooltip("Maximum rotation of the arms and camera on the axis."), SerializeField]
    private float runningSpeed = 10;

    [Tooltip("Maximum rotation of the arms and camera on the axis."), SerializeField]
    private float walkingSpeed = 5;

    [Tooltip("Approximately the amount of time it will take for the player to reach maximum running or walking speed."), SerializeField]
    private float movementSmoothness = 0.125f;

    private float jumpForce = 35f;

    CapsuleCollider _collider;

    Rigidbody _rigidbody;

    private bool _isGrounded;

    private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
    private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];

    private SmoothRotation _rotation_X;

    private SmoothRotation _rotation_Y;

    private SmoothVelocity _velocity_X;

    private SmoothVelocity _velocity_Z;

    private class SmoothRotation
    {
        float _current;
        float _currentVelocity;

        public SmoothRotation(float value)
        {
            _current = value;
        }

        public float Current
        {
            set { _current = value; }
        }

        public float UpDate(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }
    }

    private class SmoothVelocity
    {
        float _current;
        float _currentVelocity;

        public float Current
        {
            set { _current = value; }
        }

        public float UpDate(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }
    }

    /// Clamps <see cref="minVerticalAngle"/> and <see cref="maxVerticalAngle"/> to valid values and
    /// ensures that <see cref="minVerticalAngle"/> is less than <see cref="maxVerticalAngle"/>.
    private void ValidateRotationRestriction()
    {
        minVerticalAngle = ClampRotationRestriction(minVerticalAngle, -90, 90);
        maxVerticalAngle = ClampRotationRestriction(maxVerticalAngle, -90, 90);
        if (maxVerticalAngle >= minVerticalAngle) return;
        Debug.LogWarning("maxVerticalAngle should be greater than minVerticalAngle.");
        var min = minVerticalAngle;
        minVerticalAngle = maxVerticalAngle;
        maxVerticalAngle = min;
    }

    private static float ClampRotationRestriction(float rotationRestriction, float min, float max)
    {
        if (rotationRestriction >= min && rotationRestriction <= max) return rotationRestriction;
        var message = string.Format("Rotation restrictions should be between {0} and {1} degrees.", min, max);
        Debug.LogWarning(message);
        return Mathf.Clamp(rotationRestriction, min, max);
    }

    private Transform AssignCharactersCamera()
    {
        var t = transform;
        arm.SetPositionAndRotation(t.position, t.rotation);
        return arm;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        arm = AssignCharactersCamera();
        _rotation_X = new SmoothRotation(Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        _rotation_Y = new SmoothRotation(Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
        _velocity_X = new SmoothVelocity();
        _velocity_Z = new SmoothVelocity();
        Cursor.lockState = CursorLockMode.Locked;
        ValidateRotationRestriction();
        //transform.rotation = transform.rotation * Quaternion.Euler(0, 30, 0);
    }

    private void FixedUpdate()
    {
        RotateCameraAndCharacter();
        MoveCharacter();
        _isGrounded = false;
        // transform.rotation= Quaternion.AngleAxis(30, Vector3.up);
    }
    // Update is called once per frame
    void Update()
    {
        arm.position = transform.position + transform.TransformVector(armPosition);
        Jump();
    }

    private static float NormalizeAngle(float angleDegrees)//将rotation转换为相对旋转坐标
    {
        while (angleDegrees > 180f)
        {
            angleDegrees -= 360f;
        }

        while (angleDegrees <= -180f)
        {
            angleDegrees += 360f;
        }

        return angleDegrees;
    }

    private float RestrictVerticalRotation(float mouseY)
    {
        var currentAngle = NormalizeAngle(arm.eulerAngles.x);
        var minY = minVerticalAngle + currentAngle;
        var maxY = maxVerticalAngle + currentAngle;
        return Mathf.Clamp(mouseY, minY + 0.01f, maxY - 0.01f);//朝上90时max为0，故移动角返回0，下90时min为0
    }

    private void RotateCameraAndCharacter()
    {
        float Rotation_X = _rotation_X.UpDate(Input.GetAxisRaw("Mouse X") * mouseSensitivity, rotationSmoothness);
        float Rotation_Y = _rotation_Y.UpDate(Input.GetAxisRaw("Mouse Y") * mouseSensitivity, rotationSmoothness);
        float clampedY = RestrictVerticalRotation(Rotation_Y);//限制y轴转动
        _rotation_Y.Current = clampedY;
        Vector3 worldUp = arm.InverseTransformDirection(Vector3.up);//将x方向的旋转的旋转轴设置为模型的y轴
        //Quaternion rotation = arm.rotation * Quaternion.AngleAxis(Rotation_X, Vector3.up);
        Quaternion rotation = arm.rotation* Quaternion.AngleAxis(Rotation_X, worldUp) * Quaternion.AngleAxis(clampedY, Vector3.left);
        transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
        arm.rotation= rotation;
       // Debug.Log(Rotation_Y);
    }

    private void OnCollisionStay()
    {
        Bounds bounds = _collider.bounds;
        Vector3 extents = bounds.extents;
        float redius = extents.x - 0.01f;
        Physics.SphereCastNonAlloc(bounds.center, redius, Vector3.down, _groundCastResults, extents.y - redius * 0.5f, ~0, QueryTriggerInteraction.Ignore);
        if (!(_groundCastResults.Any(hit => hit.collider != null && hit.collider != _collider))) return;
        for (int i = 0; i < _groundCastResults.Length; i++)
        {
            _groundCastResults[i] = new RaycastHit();
        }
        Vector3 yd = new Vector3(bounds.center.x, bounds.center.y -( extents.y - redius * 0.5f), bounds.center.z);
        Debug.DrawLine(bounds.center, yd, Color.red);

        _isGrounded = true;
    }

    private bool CheckCollisionsWithWalls(Vector3 velocity)
    {
        if (_isGrounded) return false;
        Bounds bounds = _collider.bounds;
        float redius = _collider.radius;
        float halfHeight = _collider.height * 0.5f - redius;
        Vector3 point1 = bounds.center;
        point1.y += halfHeight;
        Vector3 point2 = bounds.center;
        point2.y -= halfHeight;
        Physics.CapsuleCastNonAlloc(point1,point2,redius,velocity, _wallCastResults,redius*0.04f,~0,QueryTriggerInteraction.Ignore);
        if (!(_wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider))) return false;
        for (int i = 0; i < _wallCastResults.Length; i++)
        {
            _wallCastResults[i] = new RaycastHit();
        }
        return true;
    }

    private void MoveCharacter()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 worldDirection = transform.TransformDirection(direction);
        Vector3 velocity = worldDirection * (Input.GetKey(KeyCode.LeftShift) ? runningSpeed : walkingSpeed);
        bool intersectsWall = CheckCollisionsWithWalls(velocity);
        if (intersectsWall)
        {
            _velocity_X.Current = _velocity_Z.Current = 0;
            return;
        }
        //Debug.Log(intersectsWall);
        float smoothX = _velocity_X.UpDate(velocity.x, movementSmoothness);
        float smoothZ = _velocity_Z.UpDate(velocity.z, movementSmoothness);
        //Debug.Log(velocity.x);
        Vector3 rigiForce = _rigidbody.velocity;
        Vector3 force = new Vector3(smoothX - rigiForce.x, 0, smoothZ - rigiForce.z);//计算加速的力的大小
        //Vector3 force = new Vector3(smoothX , 0, smoothZ );
        //Debug.Log(smoothX);
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        if (!_isGrounded || !Input.GetKeyDown(KeyCode.Space)) return;
        _isGrounded = false;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}
