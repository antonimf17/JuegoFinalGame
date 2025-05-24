using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpscam; //ref de la camara que dispara
    [SerializeField] Transform shootPoint; //Ref a la posición de la cam para disparar
    [SerializeField] RaycastHit hit; //Almacena la informacion del impacto de los raycast
    [SerializeField] LayerMask impactLayer; //Ref a la layer contra la que Si impacata el raycast
    [SerializeField] GameObject muzzleFlashPrefab;

    [Header("Weapon Parameters")]
    public int damage;
    public float range;
    public float spread;
    public float shootingCooldown;
    public float timeBetweenShoots;
    public float reloadTime;
    public bool allowButtonHold;

    [Header("Bullet Management")]
    public int ammoSize;
    public int bulletsPerTap;
    [SerializeField] int bulletsLeft;
    [SerializeField] int BulletsShot;

    [Header("State Flags")]
    [SerializeField] bool shooting;
    [SerializeField] bool canShoot = true;
    [SerializeField] bool reloading;

    public TextMeshProUGUI ammoDisplay;
    public GameObject reloadText;
    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputFlags();
        if (ammoDisplay != null)
            ammoDisplay.text = $"{bulletsLeft} / {ammoSize}";
        if (reloadText != null)
        {
            reloadText.SetActive(bulletsLeft == 0 && !reloading);
        }
    }

    void InputFlags()
    {
        if (canShoot && shooting & !reloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(flash, 1f); // Destruye el efecto después de 1 segundo para no llenar la memoria
        }

        canShoot = false;
        Vector3 direction = fpscam.transform.forward;



        if (Physics.Raycast(fpscam.transform.position, direction, out hit, range, impactLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth health = hit.collider.gameObject.GetComponent<EnemyHealth>();
                health.TakeDamage(damage);
            }
        }

        bulletsLeft--;
        BulletsShot++;

        if (!IsInvoking(nameof(ResetShoot)) & !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);
        }

    }
   
    private void ResetShoot()
    {
        canShoot = true;
    }

    private void Reload()
    {
        if (bulletsLeft < ammoSize && !reloading)
        {
            reloading = true;
            Invoke(nameof(ReloadFinished), reloadTime);

        }
        else Debug.Log("Can't reload now!");
        
    }
    private void ReloadFinished()
    {
        bulletsLeft = ammoSize;
        BulletsShot = 0;
        reloading = false;
        reloadText?.SetActive(false);
    }
    #region input Methods
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shooting = true;
        }
        if (context.canceled)
        {
            shooting = false;
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed) Reload();
    }
}
#endregion