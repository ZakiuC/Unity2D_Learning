using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  相机控制类
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [Header("相机移动速度")]
    public float speed;

    private void Awake() 
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        }
        else
        {
            Debug.Log("CameraController: Can't find GameObject: Player.");
        }
    }

    private void FixedUpdate() {
        Move();
    }

    /// <summary>
    /// 更新相机位置
    /// </summary>
    public void Move()
    {
        Vector3 endPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * speed);
    }
}
