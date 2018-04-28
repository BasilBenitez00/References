using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public int bulletPerMag = 30;				// max bullet capacity per clip/magazine
	public int totalAmmo;						// total ammo reserve
	public int currentBullets;					// amount of bullets in clip
	public float rateOfFire = 0.3f;
	public float range;							// max range of the rifle
	public float gunDamage;
	public Transform shootPoint;
	public ParticleSystem muzzleFlash; 			// muzzle flash
	public AudioClip fireSound;					// fire sound 
	public bool isReloading;
	float shootRate;

	Animator myAnim;
	AudioSource audioSource;



	// Use this for initialization
	void Start () 
	{
		myAnim = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		currentBullets = bulletPerMag;	
	}

	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (isReloading);
		if(Input.GetButton("Fire1"))
		{
			if(currentBullets > 0)
			{
				Fire ();													// For Shooting by pressing left mouse button :3
			}
			else if(totalAmmo > 0)
			{
				DoReload ();  												// if there is no bullet left in the mag, do reload kek
			}												
		}	
		if(Input.GetKeyDown(KeyCode.R))										//reloading
		{
			if(currentBullets < bulletPerMag && totalAmmo > 0)				
			{
				DoReload ();
			}
		}

		if(shootRate < rateOfFire)											// rate of fire, for keeping it in check.			
		{
			shootRate += Time.deltaTime;
		}
	}

	void Fire()
	{
		if (shootRate < rateOfFire || currentBullets <= 0 || isReloading)
			return;



		RaycastHit hit;
	
		if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) 		//raycast for shooting
		{
			Debug.DrawLine (shootPoint.position, hit.point, Color.red);
			if(hit.transform.tag == "Zombie" || hit.transform.tag == "Demon")
			{
				hit.transform.GetComponent<ZombieHealth> ().AddDamage (gunDamage);
			}

		}

		myAnim.CrossFadeInFixedTime ("ShootAnim", 0.01f);
		muzzleFlash.Play ();
		PlayShootSound ();
		currentBullets--;																		
		shootRate = 0f;														//reset shoot rate
	}

	public void Reload()													//actual reload function
	{
		if (totalAmmo < 0)    												// if ammo reserve is empty return :3
			return;
		int bulletsToLoad = bulletPerMag - currentBullets;  									
		int bulletsToDeduct = (totalAmmo >= bulletsToLoad) ? bulletsToLoad : totalAmmo;	// check if ammo reserve is sufficient enough to reload a full clip lelelele

		totalAmmo -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;

	}

	void DoReload()															// reload Animations
	{
		if (isReloading)													// return if animation is already running
			return;
		
		myAnim.CrossFadeInFixedTime ("Reload", 0.01f);
		isReloading = false;
	}

	void PlayShootSound()													//shoot sound
	{
		//audioSource.clip = fireSound;
		audioSource.PlayOneShot (fireSound);
	}


	public void AddAmmo(int amount)
	{
		totalAmmo += amount;
		//Play PickUp Sound
	}


}
