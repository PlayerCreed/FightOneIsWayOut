using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCtr : MonoBehaviour
{
    public static AmmoCtr Instance { get; private set; }
    [System.Serializable]
    public struct arm
    {
        public string armName;
        public int magazineClipAmmo;//弹夹目前弹药
        public int magazineClip;//弹夹额定弹药
        public int backupAmmo;//备弹
        public int maxAmmo;//最大携带弹药
    }

    private GameObject gameCtr;

    public arm[] armList;

    public bool ammoSupply;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameCtr = GameObject.Find("SystemCanvas");
    }

    private void Update()
    {
        if (gameCtr.GetComponent<GameCtr>().bulletsInfiniteFlag)
        {
            for (int i = 0; i < armList.Length; i++)
            {
                armList[i].magazineClipAmmo = armList[i].magazineClip;
            }
        }
        if (ammoSupply)
        {
            for (int i = 0; i < armList.Length; i++)
            {
                armList[i].backupAmmo = armList[i].maxAmmo;
            }
        }
    }
}
