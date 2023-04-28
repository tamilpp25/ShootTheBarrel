using UnityEngine.InputSystem;
using UnityEngine;
using StarterAssets;

public class FireWeapon : MonoBehaviour
{
    private StarterAssetsInputs inputs;

    [SerializeField]
    private GameObject bulletPos;

    public Transform cameraPos;
    public GameObject muzzleFlash;
    public GameObject explosionPrefab;

    [SerializeField]
    private GameObject bullet;

    public int force = 360;
    public int rayCastRange = 20;
    public int hitForce = 20;
    // Start is called before the first frame update
    void Start()
    {
        inputs = GameObject.Find("PlayerCapsule").GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.shoot)
        {
            Debug.Log("SHOT!");
            inputs.shoot = false;

            GameObject firedBullet = Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
            firedBullet.GetComponent<Rigidbody>().AddForce(bulletPos.transform.forward * force);
            GameObject flash = Instantiate(muzzleFlash, bulletPos.transform.position, bulletPos.transform.rotation);
            flash.GetComponent<Transform>().SetParent(bulletPos.transform);
            Destroy(flash, 0.15f);
            Destroy(firedBullet, 5);


            //transform.root.root.GetComponent<Rigidbody>().AddForce(-transform.forward * hitForce);

            RaycastHit hitInfo;
            if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hitInfo, rayCastRange))
            {
                if(hitInfo.rigidbody != null && hitInfo.transform.gameObject.tag == "Damagable")
                {
                    hitInfo.rigidbody.AddForce(-hitInfo.normal * hitForce);
                    GameObject explode = Instantiate(explosionPrefab, hitInfo.transform.position, Quaternion.identity);
                    explode.GetComponent<Transform>().SetParent(bulletPos.transform);
                    Destroy(hitInfo.rigidbody.gameObject, 0.5f);
                    Destroy(explode, 2f);
                    Debug.Log("HITTED BARREL");
                    GameManager.instance.points++;
                    GameManager.instance.CalculatePoints();
                    GameManager.instance.barrelsBroken++;
                }
            }
        }
    }
}
