using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsSwitching : MonoBehaviour {

	public GameObject weapon01;									//Actual Weapons
	public GameObject weapon02;
	public GameObject weapon03;

	public Image weapon1Icon;								//Weapon Icons UI
	public Image weapon2Icon;
	public Image weapon3Icon;

	public Text ammoInfo;									// ammo text

	Shooting shoot;
	GameManager _GM;

	// Use this for initialization
	void Start () 
	{
		_GM = FindObjectOfType<GameManager> ();
		weapon01.SetActive (true);
		weapon02.SetActive (false);
		weapon03.SetActive (false);

		weapon1Icon.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(weapon01.activeInHierarchy)								//basically check which weapons is active then get their respective shooting scripts
		{
			shoot = weapon01.GetComponent<Shooting> ();

			ammoInfo.text = "" + shoot.currentBullets + "/" + "999";		// infinite ammo
		}
		else if(weapon02.activeInHierarchy)
		{
			shoot = weapon02.GetComponent<Shooting> ();
			ammoInfo.text = "" + shoot.currentBullets + "/" + shoot.totalAmmo;	//show ammo info
		}
		else if (weapon03.activeInHierarchy)
		{
			shoot = weapon03.GetComponent<Shooting> ();
			ammoInfo.text = "" + shoot.currentBullets + "/" + shoot.totalAmmo;	
		}

		if(Input.GetKeyDown(KeyCode.Alpha1) && !shoot.isReloading)		// check if its currently reloading, avoiding bugs when reloading
		{
			
			weapon01.SetActive (true);									// Set the Pistol (weapon01) Active and disable others
			weapon02.SetActive (false);
			weapon03.SetActive (false);

			weapon1Icon.gameObject.SetActive (true);					// show Icon of the current active weapon
			weapon2Icon.gameObject.SetActive (false);
			weapon3Icon.gameObject.SetActive (false);


		}	
		if(Input.GetKeyDown(KeyCode.Alpha2) && !shoot.isReloading && _GM.waveLevel >= 3) // wait to reach level 3
		{
			weapon01.SetActive (false);
			weapon02.SetActive (true);									// Set the SMG (weapon02) Active and disable others
			weapon03.SetActive (false);

			weapon2Icon.gameObject.SetActive (true);					// show Icon of the current active weapon
			weapon1Icon.gameObject.SetActive (false);
			weapon3Icon.gameObject.SetActive (false);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3) && !shoot.isReloading && _GM.waveLevel >= 5) // wait to reach level 5
		{
			weapon01.SetActive (false);
			weapon02.SetActive (false);
			weapon03.SetActive (true);									// Set the AK (weapon03) Active and disable others

			weapon3Icon.gameObject.SetActive (true);					// show Icon of the current active weapon
			weapon1Icon.gameObject.SetActive (false);
			weapon2Icon.gameObject.SetActive (false);
		}
	}

}
