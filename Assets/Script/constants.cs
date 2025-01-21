using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constants : MonoBehaviour
{
    public float CubeW;
    public float CubeH;
    public float CubeD;

    public float TallW;
    public float TallH;
    public float TallD;

    public float ToolW;
    public float ToolH;
    public float ToolD;

    public float wall_mount_32W;
    public float wall_mount_32H;
    public float wall_mount_32D;

    public float wall_mount_33W;
    public float wall_mount_33H;
    public float wall_mount_33D;

    public float wall_mount_62W;
    public float wall_mount_62H;
    public float wall_mount_62D;

    public float BenchW;
    public float BenchH;
    public float BenchD;



    /*-----------new--------------*/

    public float TollBoyW;
    public float TollBoyH;
    public float TollBoyD;

    public float BaseW;
    public float BaseH;
    public float BaseD;

    public float TallCabinetW;
    public float TallCabinetH;
    public float TallCabinetD;

    public float WallMountW;
    public float WallMountH;
    public float WallMountD;

    public float OverheadW;
    public float OverheadH;
    public float OverheadD;

    public float BackSplashH;
    public float WorkSurfaceH;

    public static constants instance;

    private void Awake()
    {
        instance = this;
    }


}
