﻿using UnityEngine;
using TMPro;


public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;

    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;

    [SerializeField] private TextMeshProUGUI ammoText;

    void Start()
    {
        mainCam = Camera.main;
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        Ammo.OnAmmoCollect += AddAmmo;
    }

    public void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        // Šaudymo sąlyga – atskirta į testuojamą metodą
        if (Input.GetMouseButton(0))
        {
            TryFireManually();
        }

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // 🔥 Šis metodas bus naudojamas testuose vietoj `Input.GetMouseButton(0)`
    public void TryFireManually()
    {
        if (canFire && currentAmmo > 0)
        {
            canFire = false;
            SoundManager.Instance.PlayGunSound();
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }

    private void AddAmmo(int amount)
    {
        currentAmmo += amount;
    }
}
