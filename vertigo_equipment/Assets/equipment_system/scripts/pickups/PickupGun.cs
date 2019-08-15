using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : PickupInteract
{
    public GameObject projectile;
    public float projectileForce = 1000;
    public float projectileLifespan = 30;
    public int projectileLimit = 30;
    public Material singleModeMat, autoModeMat;

    private int projectileCount = 0;
    private bool isFullAuto, isPrimaryUseDown = false;
    private bool isValidProjectile = true;
    private Transform projectileSpawn;
    private MeshRenderer gunRender;
    private TextMesh ProjectileText;
    private const string projectileSpawnName = "ProjectileSpawn";

    private new void Start()
    {
        base.Start();
        projectileCount = projectileLimit;
        if(!projectile || !projectile.GetComponent<Rigidbody>())
        {
            Debug.LogWarning("Projectile has no rigidbody, gun won't function!");
            isValidProjectile = false;
        }

        // Set spawn to transform in case it cannot find a spawn transform
        projectileSpawn = transform;
        if(transform.Find(projectileSpawnName))
        {
            projectileSpawn = transform.Find(projectileSpawnName);
        }

        gunRender = GetComponentInChildren<MeshRenderer>();
        if(gunRender)
        {
            gunRender.material = isFullAuto ? autoModeMat : singleModeMat;
        }
        ProjectileText = GetComponentInChildren<TextMesh>();
        UpdateText();
    }

    private void Update()
    {
        if (isPrimaryUseDown && isFullAuto)
        {
            if (!transform.parent)
            {
                isPrimaryUseDown = false;
            }
            else
            {
                PrimaryUseDown();
            }
        }
    }

    public override bool PrimaryUseDown()
    {
        if(isValidProjectile && projectileCount > 0)
        {
            GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * Time.fixedDeltaTime * projectileForce, ForceMode.VelocityChange);
            Destroy(newProjectile, projectileLifespan);
            projectileCount--;
            isPrimaryUseDown = true;
            UpdateText();
        }
        return true;
    }

    public override bool PrimaryUseUp()
    {
        isPrimaryUseDown = false;
        return true;
    }

    public override bool SecondaryUseDown()
    {
        isFullAuto = !isFullAuto;
        if(gunRender)
        {
            gunRender.material = isFullAuto ? autoModeMat : singleModeMat;
        }
        return true;
    }

    public bool Reload()
    {
        if(projectileCount >= projectileLimit)
        {
            return false;
        }
        projectileCount = projectileLimit;
        UpdateText();
        return true;
    }

    private void UpdateText()
    {
        if (ProjectileText)
        {
            ProjectileText.text = projectileCount.ToString();
        }
    }
}
