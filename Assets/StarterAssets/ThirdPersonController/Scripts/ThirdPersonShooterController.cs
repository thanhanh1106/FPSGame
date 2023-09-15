using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float normalSensitivity;
    [SerializeField] float aimSensitivity;
    [SerializeField] LayerMask aimLayerMask;
    [SerializeField] Transform spawnBulletPoint;
    [SerializeField] BulletPool bulletPool;
    [SerializeField] Transform aimPoint;
    [SerializeField] Rig aimRig;

    StarterAssetsInputs starterAssetsInputs;
    ThirdPersonController personController;
    Animator animator;

    // public bool IsAiming { get;private set; }
    float aimRigWeight;

    Vector2 screenCenterPoint;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        personController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        screenCenterPoint = new Vector2 (Screen.width/2f, Screen.height/2f);
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, aimLayerMask))
        {
            mouseWorldPosition = hitInfo.point;
        }
        aimPoint.position = mouseWorldPosition;

        // xử lý khi bấm nút aim
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            personController.SetSensitivity(aimSensitivity);
            personController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),1f,Time.deltaTime*10f));// khi aim thì cho chạy layer aim
            aimRigWeight = 1f;

            // cho nhân vật hướng về phía tâm ngắm
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            Vector3 velocity = Vector3.zero;
            transform.forward = Vector3.SmoothDamp(transform.forward, aimDirection,ref velocity, 0.02f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            personController.SetSensitivity(normalSensitivity);
            personController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f)); //ngừng aim thì set nó về 0 để chạy layer base
            aimRigWeight = 0f;
        }

        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);

        if (starterAssetsInputs.shoot && starterAssetsInputs.aim)
        {
            Vector3 aimDiretion = (mouseWorldPosition - spawnBulletPoint.position).normalized;
            BulletProjectile bullet = bulletPool.GetBullet(spawnBulletPoint.position).GetComponent<BulletProjectile>();
            bullet.SetDiretion(aimDiretion);
            starterAssetsInputs.shoot = false;
        }
    }
}
