using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using VRC.SDK3.Components;
using UnityEngine.XR;
public enum fireSelection
{
    Safe,
    Semi,
    Auto,
    Burst
}

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
[AddComponentMenu("")]

public class Gun : UdonSharpBehaviour
{
    public bool infiniteAmmo;
    [Tooltip("Max distance bullet can travel")][SerializeField] private float maxDistance;

    public float RaycastDamageAmount;
    [Tooltip("If True, Gun Will Shoot Multiple Pellets Like A Shotgun")][SerializeField] private bool shotgun = false;
    [Tooltip("If Shotgun Is True How Many Pellets Per-Shot")][SerializeField] private int pelletCount;
    [Tooltip("Reload animations based on how many shells get manually inserted.")][SerializeField] private bool perShellReload = false;
    //[Range(1f, 10f)][SerializeField] private int speedUpFactor = 0;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float BulletSpread = 10f;
    [Tooltip("Current Ammo In Mag")][UdonSynced] public int AmmoCount = 5;
    [Tooltip("Static Mag Size")] public int MagSize = 10;
    [Tooltip("Ammo Reserves")] public int MaxAmmo = 10;


    //[Header("Fire Selection Settings")]
    [Tooltip("Fire Mode Type (Burst, Auto, Semi")] public fireSelection fireSelection = fireSelection.Semi;
    [Tooltip("This Will Base The Reload Time Off Of How Long The Per-Shot/Cycle Animation")] public bool BaseCycleOffAnimation;
    [Tooltip("Rate Of Fire/Cycle Between Bursts")] public float CycleTime;
    [Tooltip("Burst Rate Of Fire")] public float BurstCycleTime;
    [Tooltip("Bullets Per Burst")] public int BurstCount;
    private int currentBurstCount;
    //[Tooltip("If true the gun will play special shoot/cycle animation when on last bullet in mag")][SerializeField] private bool seperateFinalShotAnimationToggle = false;
    //[Tooltip("If true the gun will play special shoot/cycle sound effect when on last bullet in mag")][SerializeField] private bool seperateFinalShot = false;
    //[Header("Audio Settings")]
    public AudioSource barrelAudioSource;
    public AudioSource magazineAudioSource;
    public AudioSource fireMechanismAudioSource;
    //foldout
    [SerializeField] private AudioClip gunShot;
    [Tooltip("Sound clip that only plays when you shoot with one bullet in mag (leave this empty to always use the other shoot sound regardless of current live mag size)")][SerializeField] private AudioClip finalGunShotClip;
    [Tooltip("Random sound clip is picked from the array and played when gun is fired (works with final shot audio [final shot will always be final shot if its being used])")][SerializeField] private AudioClip[] randomGunShotClip;
    [SerializeField] private AudioClip MagPull;
    [SerializeField] private AudioClip MagInsert;
    [SerializeField] private AudioClip GunCock;
    [SerializeField] private AudioClip GunEmpty;


    //[Header("Reload Settings")]
    public float reloadTime = 15f;
    [Tooltip("Bases the reload time off of the length of the animation")] public bool BaseReloadTimeOffAnimation;
    [SerializeField] private bool reloadOnMagEmpty = true;
    [HideInInspector][SerializeField] private bool bulletInChamberAddsToAmmoCount = false; //dont use this!!!
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimationOffset = 0.0f;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float finalReloadAnimationOffset = 0.0f;

    //[Header("Animation Settings")]
    [Tooltip("Gun model's animation controller (without this the script wont know where to play the animations from)")] public Animator GunAnimator;
    [Tooltip("Animation Clip That Plays Per Shot")] public AnimationClip CycleAnimation;
    [Tooltip("Animation clip that only plays when you shoot the last bullet in mag (leave this empty to always use the other shoot animation regardless of current live mag size)")] public AnimationClip finalCycleAnimation;
    [Tooltip("Wont play any reloading audio clips, this is designed for when you play the clips via external script in the animation through events")] [SerializeField] private bool reloadAudioBypass = false;
    [Tooltip("Animation clip that plays when you reload")] public AnimationClip ReloadAnimation;
    //[Tooltip("if true a seperate animation will play on reload if theres no ammo")][SerializeField] private bool seperateReloadAnimation = false;
    [Tooltip("Animation clip that only plays when you reload with no bullets in mag (leave this empty to always use the other reload animation regardless of current live mag size)")] public AnimationClip finalReloadAnimation;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation1;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation1Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation2;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation2Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation3;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation3Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation4;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation4Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation5;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation5Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation6;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation6Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation7;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation7Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation8;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation8Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation9;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation9Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation10;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation10Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation11;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation11Offset = 0.0f;
    [Tooltip("Animation clip that plays on reload depending on how many shells you need to insert")] public AnimationClip ReloadAnimation12;
    [Tooltip("Seconds in which the reloading time is reduced by")][SerializeField] private float reloadAnimation12Offset = 0.0f;

    //[Header("Animator Settings For Gun Belts/Per Shot Updates")]
    [Tooltip("Updates Variable Per Shot")] public Animator magazineAnimator;
    [Tooltip("Updates Variable Per Shot")] public string MagazineAnimatorVariable = "AmmoPercentage";

    private float firedTime;
    private int MagazineAnimatorVariableHash;
    private bool reloading;
    private float reloadTimer;

    //individual timers for each reloading audioclip
    private float magPullTimer;
    private float magInsertTimer;
    private float gunCockTimer;
    private int lastClipIndex = -1;

    //this timer is for when the mag should be inserted
    private float magInsertTimeStamp;

    //bools tracking which stage of reloading is done
    private bool magPulled;
    private bool magInserted;
    private bool gunCocked;

    [UdonSynced] private bool bulletInChamber = true;


    //[Header("Addons")]
    [Tooltip("Ammo Count Text Output")] public TextMeshProUGUI Display;
    [HideInInspector]public HUDAmmoCount ammoCountHud;
    [Tooltip("Gunshell Particle Effect (Per-Shot)")] public ParticleSystem ShellParticle;
    //[Tooltip("Gunshell Particle Effect (Per-Shot)")] public ParticleSystem BoltActionShellParticle;
    [Tooltip("Muzzle Flash Particle Effect (Per-Shot)")] public ParticleSystem gunShotParticle;

    //[Header("Raycast Settings")]
    [Tooltip("Turn Off When Using Physical Projectiles")] public bool RaycastDamage = true;
    [Tooltip("When On Bullets Will Drop Over Distance")][SerializeField] private bool RaycastBulletDrop;
    [Tooltip("When On Desktop Users Will Shoot From Their Crosshair")][SerializeField] private bool desktopFaceFiring;
    [Tooltip("Particle Effect That Plays When Shooting A Player")][SerializeField] private GameObject playerRaycastSpark;
    [Tooltip("Layers For Players")][SerializeField] private LayerMask players;
    [Tooltip("Particle Effect That Plays When Shooting The Ground")][SerializeField] private GameObject terrainRaycastSpark;
    [Tooltip("Layers For Ground")][SerializeField] private LayerMask sparkable;

    //[Header("Haptic Feedback")]
    public bool hapticFeedback;
    [SerializeField] private float hapticFeedbackDuration = 0.1f;
    [SerializeField] private float hapticFeedbackAmplitude = 1;
    [SerializeField] private float hapticFeedbackFrequency = 1;
    [SerializeField] private bool hapticFeedbackOnManualReload;
    [SerializeField] private VRC_Pickup secondaryGripPickup;
    [SerializeField] private VRC_Pickup pickup;

    //[Header("Rigibody Projectile, Requires Raycast To Be Off")]
    public GameObject bullet;
    public float fireVelocity = 5f;

    //[Header("mMulti-Barrel Settings, If The Script Detects mMultiple Barrels It Will Use Them")]
    [SerializeField] private Transform[] barrels;
    [SerializeField] private bool fireAllBarrelsAtOnce;
    public bool ammoCountDependsOnBarrels;
    private int currentBarrel = 0;

    
    //[Header("Physics Stuff, Only Add If You're Putting This On A Vehicle")]
    [SerializeField] Rigidbody targetRigidbody;
    [SerializeField] private float forceFromBarrel;
    
    //[Header("Manipulation Settings")]
    public bool useVirtualStock;
    [SerializeField] private Transform stockTransform;
    [SerializeField] private Transform rotationPointTransform;
    [SerializeField] private float relativeVirtualStockActivationDistance;
    [SerializeField] private Vector2 rotationRange = new Vector2(180, 180);

    [HideInInspector] public bool AmmoCheck = true;

    private VRCPlayerApi localPlayer;
    private bool DesktopUser;

    private int MAX_MAG_SIZE;
    [HideInInspector] public int MAX_RESERVE_AMMO;
    private int ammoLeftOver;


    private string AnimName;
    private bool Firing = false;
    private bool Scoped;
    private VRC_Pickup.PickupHand hand;
    [UdonSynced] private float playerHeight;

    public bool isMagFull()
    {
        if (AmmoCount == MAX_MAG_SIZE) return true;

        else return false;

    }


    public void TriggerPull()
    {
        //switch case for different fire modes

        if (AmmoCount > 0 && !reloading)
        {
            switch (fireSelection)
            {
                case fireSelection.Safe:
                    break;
                case fireSelection.Semi:
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Fire");
                    break;
                case fireSelection.Auto:
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetFireTrue");
                    break;
                case fireSelection.Burst:
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "FireBurst");
                    break;
            }
        }
        else
        {
            if(fireMechanismAudioSource && GunEmpty && !reloading)
            {
                fireMechanismAudioSource.PlayOneShot(GunEmpty);
            }
            //bulletInChamber = false;
        }
    }
    public void TriggerRelease()
    {
        if (fireSelection == fireSelection.Auto)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetFireFalse");
        }
    }

    public void OnDisable()
    {
        AmmoCount = MaxAmmo;
    }
    public void FireBurst()
    {
        fireSelection = fireSelection.Burst;
        currentBurstCount = BurstCount;
        Firing = true;
    }
    public void SetFireTrue()
    {
        fireSelection = fireSelection.Auto;
        Firing = true;
    }

    public void SetFireFalse()
    {
        Firing = false;
    }
    private void Update()
    {

        if (Firing == true)
        {
            if (Time.time - firedTime > CycleTime)
            {
                Fire();
                if(fireSelection == fireSelection.Burst)
                {
                    currentBurstCount--;
                    if(currentBurstCount == 1)
                    {
                        Firing = false;
                    }
                }
            }
        }
        if(pickup)
        {   // Manual Reload
            if (Input.GetKey(KeyCode.Q) && !isMagFull() && !reloading && MaxAmmo > 0 && pickup.IsHeld && Time.time - firedTime > CycleTime && localPlayer == Networking.GetOwner(gameObject))
            {
                reloading = true;
                Reload();
            }
        }

        
        if (!reloading && AmmoCount > 0 && Display != null)
        {
            Display.text = (AmmoCount + " / " + MaxAmmo);
        }

        if (!reloading && AmmoCount <= 0 && Display != null)
        {
            Display.text = (AmmoCount + " / " + MaxAmmo);
        }

        // Auto Reload 
        if (AmmoCount <= 0 && !reloading && reloadOnMagEmpty && MaxAmmo > 0 && Time.time - firedTime > CycleTime)
        {
            bulletInChamber = false;
            reloading = true;
            Reload();
        }

        //TODO all the seperateTimers for reload
        if (reloading)
        {

            /*
            if (Time.time - reloadTimer > reloadTime)
            {
                AmmoCount = MaxAmmo;
                AmmoCheck = true;
            }*/

            //timer to wait for the mag pull audio to finish
            if (!magPulled)
            {
                if (Time.time - magPullTimer > MagPull.length)
                {
                    magPulled = true;
                    magInserted = false;
                    
                }
            }

            //magInsert timer

            //check if the maginserttimestamp has passed
            if(Time.time > magInsertTimeStamp)
            {
                if (!magInserted)
                {
                    magInserted = true;
                    magInsertTimer = MagInsert.length+1;
                    if (MagInsert && magazineAudioSource)
                    {
                        if (!reloadAudioBypass)
                        {
                            magazineAudioSource.PlayOneShot(MagInsert);
                        }
                        //magazineAudioSource.PlayOneShot(MagInsert);
                        Debug.Log("mag insert audio played");
                    }
                }
                if (magPulled &&magInserted)
                {
                    if (magInsertTimer > 0)
                    {
                        magInsertTimer -= Time.deltaTime;
                    }
                    else
                    {
                        magPulled = false;
                        if (!bulletInChamber)
                        {
                            //timer over, play the mag gun cock;
                            if(GunCock && fireMechanismAudioSource&&!gunCocked)
                            {
                                if (!reloadAudioBypass)
                                {
                                    fireMechanismAudioSource.PlayOneShot(GunCock);
                                }
                                //fireMechanismAudioSource.PlayOneShot(GunCock);
                                Debug.Log("gun cock audio played");
                                gunCocked = true;
                                bulletInChamber = true;
                            }

                        }
                        else
                        {
                            reloading = false;


                            if (MaxAmmo >= MagSize)
                            {
                                ammoLeftOver = AmmoCount;
                                AmmoCount = MagSize;
                                MaxAmmo -= (MagSize - ammoLeftOver);
                            }

                            else
                            {
                                AmmoCount = MaxAmmo;
                                MaxAmmo = 0;
                            }


                            if (MaxAmmo <= 0) // Makes sure reserve ammo is not negative // AKA Insurance
                            {
                                MaxAmmo = 0;
                            }



                            /* if (AmmoCount >= 1) // Lame ass code // DO NOT TOUCH BUDDY //
                             {
                                 ammoLeftOver = AmmoCount;
                                 AmmoCount += MaxAmmo;
                                 AmmoCount = MagSize;
                                 MaxAmmo -= (MagSize - ammoLeftOver);
                                 Debug.Log("IF BLOCK PLAYIN");
                             }


                             if (AmmoCount < 1)
                             {
                                 AmmoCount = MagSize;
                                 MaxAmmo -= MagSize;
                                 Debug.Log("ELSE BLOCK PLAYIN");
                             }

                             */




                            /* if (bulletInChamberAddsToAmmoCount)
                             {
                                 MaxAmmo -= reloadAmount + AmmoCount;
                                 AmmoCount = MaxAmmo + 1;

                             }
                             else
                             {
                                 MaxAmmo -= reloadAmount + AmmoCount;
                                 AmmoCount = MaxAmmo;
                             } */

                        }

                    }

                }
            }
            
           
            //gun cock timer
            if(magInserted && !magPulled)
            {
                if(gunCockTimer>0)
                {
                    gunCockTimer -= Time.deltaTime;
                }
                else
                {
                    if(!bulletInChamber)
                    {
                        bulletInChamber = true;
                        //set ammo to max
                        AmmoCount = MaxAmmo;
                    }
                    reloading = false;
                }
            }

        }

    }
    public float GetAvatarHeight(VRCPlayerApi player)
    {
        float height = 0;
        Vector3 postition1 = player.GetBonePosition(HumanBodyBones.Head);
        Vector3 postition2 = player.GetBonePosition(HumanBodyBones.Neck);
        height += (postition1 - postition2).magnitude;
        postition1 = postition2;
        postition2 = player.GetBonePosition(HumanBodyBones.Hips);
        height += (postition1 - postition2).magnitude;
        postition1 = postition2;
        postition2 = player.GetBonePosition(HumanBodyBones.RightLowerLeg);
        height += (postition1 - postition2).magnitude;
        postition1 = postition2;
        postition2 = player.GetBonePosition(HumanBodyBones.RightFoot);
        height += (postition1 - postition2).magnitude;
        return height;
    }
    private void Start()
    {
        // Keeps Value of Max Mag Size // 
        MAX_MAG_SIZE = MagSize;
        MAX_RESERVE_AMMO = MaxAmmo;

        localPlayer = Networking.LocalPlayer;
        if (magazineAnimator)
        {
            MagazineAnimatorVariableHash = Animator.StringToHash(MagazineAnimatorVariable);
        }
        //audio = (AudioSource)gameObject.GetComponent(typeof(AudioSource));
        if (!localPlayer.IsUserInVR())
        {
            DesktopUser = true;
        }
        else
        {
            DesktopUser = false;
        }

        
        Debug.Log("Ammo Left: " + AmmoCount);

        if(ammoCountDependsOnBarrels)
        {
            AmmoCount = barrels.Length;
        }
        //MaxAmmo = AmmoCount;

        if (BaseCycleOffAnimation&&CycleAnimation)
        {
            CycleTime = CycleAnimation.length;
        }
        if (BaseReloadTimeOffAnimation && ReloadAnimation)
        {
            reloadTime = ReloadAnimation.length;
        }
        if(secondaryGripPickup)
        {
            secondaryGripPickup.pickupable = false;
        }
    }
    public void Pickup()
    {
        //owner = localPlayer;
        
        if(secondaryGripPickup != null)
        {
            secondaryGripPickup.pickupable = true;
            Networking.SetOwner(localPlayer, secondaryGripPickup.gameObject);
        }
        //find which hand the current pickup is in
        if (!pickup)
            return;
        if (localPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left) == pickup)
        {
            hand = VRC_Pickup.PickupHand.Left;
        }
        else if (localPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right) == pickup)
        {
            hand = VRC_Pickup.PickupHand.Right;
        }
        else
        {
            hand = VRC_Pickup.PickupHand.None;
        }
        playerHeight = GetAvatarHeight(localPlayer);
        RequestSerialization();
    }

    public void Drop()
    {
        if(secondaryGripPickup != null)
        {
            secondaryGripPickup.pickupable = false;
            
            if (secondaryGripPickup.IsHeld)
            {
                //retrieve the gripscript from the secondardgrip gameobject
                GripScript gripScript = secondaryGripPickup.GetComponent<GripScript>();
                //tell the gripscript to attach the gun to itself]
                gripScript.AttachGunToSelf();
            }
            else
            {
                
            }
        }
        RequestSerialization();
    }
    public void BulletRaycast()
    {
        Vector3 startPoint = firePosition.position;
        Vector3 velocity = firePosition.forward * fireVelocity;
        RaycastHit hit;
        int iterations = 100;
        float timeStep = 0.01f;
        if(RaycastBulletDrop)
        {
            for (int ii = 1; ii < iterations; ii++)
            {
                Debug.DrawLine(startPoint, startPoint + velocity * timeStep);
                startPoint += velocity * timeStep;
                // Detect collision

                if (Physics.Raycast(startPoint, velocity, out hit, velocity.magnitude * timeStep, players))
                {
                   
                        Debug.Log("player layer hit");
                        if (hit.transform.gameObject.name.Contains("hitbox"))
                        {
                            if (localPlayer.IsOwner(gameObject))
                            {
                                GameObject target = hit.collider.gameObject;
                                UdonBehaviour TargetBehaviour = (UdonBehaviour)target.GetComponent(typeof(UdonBehaviour));
                                Debug.Log("before Modification");
                                TargetBehaviour.SetProgramVariable("Modifier", (RaycastDamageAmount * -1));
                                Debug.Log("aftermodification");
                                TargetBehaviour.SendCustomEvent("ModifyHealth");
                            }
                            if (playerRaycastSpark != null)
                            {
                                var spark = VRCInstantiate(playerRaycastSpark);
                                spark.transform.position = hit.point;
                                spark.SetActive(true);
                            }
                        }
                }
                if (Physics.Raycast(startPoint, velocity, out hit, velocity.magnitude * timeStep, sparkable))
                {
                    Debug.Log("raycast hit");
                    if (hit.collider != null)
                    {
                        if (!hit.collider.isTrigger)
                        {
                            if (terrainRaycastSpark != null)
                            {
                                var spark = VRCInstantiate(terrainRaycastSpark);
                                spark.transform.position = hit.point;
                                spark.SetActive(true);
                                Debug.Log("wall hit");
                            }
                            Debug.Log("wall hit");

                        }
                    }
                }
                    velocity.y -= 9.81f * timeStep; // simulate gravitational acceleration
            }
        }
        else
        {
            if ((Physics.Raycast(firePosition.position, firePosition.forward, out hit, maxDistance, players)))
            {

                Debug.Log("player layer hit");
                if (hit.transform.gameObject.name.Contains("hitbox"))
                {
                    if (localPlayer.IsOwner(gameObject))
                    {
                        GameObject target = hit.collider.gameObject;
                        UdonBehaviour TargetBehaviour = (UdonBehaviour)target.GetComponent(typeof(UdonBehaviour));
                        Debug.Log("before Modification");
                        TargetBehaviour.SetProgramVariable("Modifier", (RaycastDamageAmount * -1));
                        Debug.Log("aftermodification");
                        TargetBehaviour.SendCustomEvent("ModifyHealth");
                    }
                    if (playerRaycastSpark != null)
                    {
                        var spark = VRCInstantiate(playerRaycastSpark);
                        spark.transform.position = hit.point;
                        spark.SetActive(true);
                    }
                }
                
            }
            if ((Physics.Raycast(firePosition.position, firePosition.forward, out hit, maxDistance, sparkable)))
            {
                Debug.Log("raycast hit");
                    if (hit.collider != null)
                    {
                        if (!hit.collider.isTrigger)
                        {
                            if (terrainRaycastSpark != null)
                            {
                                var spark = VRCInstantiate(terrainRaycastSpark);
                                spark.transform.position = hit.point;
                                spark.SetActive(true);
                                Debug.Log("wall hit");
                            }
                            Debug.Log("wall hit");
                        }
                    }
                
            }
        } 
    }
    public void Fire()
    {
       
        if(barrels.Length!=0)
        {
            firePosition.position = barrels[currentBarrel].position;
            Debug.Log("fire position changed to new barrel " + currentBarrel);
            //select the next available current barrel
            if (currentBarrel < barrels.Length - 1)
            {
                currentBarrel++;
            }
            else
            {
                currentBarrel = 0;
            }
        }
        
        if (Time.time - firedTime < CycleTime)
        {
            Debug.Log("fired too fast");
            return;
        }
        firedTime = Time.time;
        if (infiniteAmmo)
        {
            AmmoCount++;
        }
        if(magazineAnimator)
        {
            magazineAnimator.SetFloat(MagazineAnimatorVariableHash, AmmoCount / MaxAmmo);
        }
        if (AmmoCount > 0)
        {
            if (hapticFeedback && pickup && Networking.LocalPlayer == Networking.GetOwner(gameObject))
            {
                Networking.LocalPlayer.PlayHapticEventInHand(hand, hapticFeedbackDuration, hapticFeedbackAmplitude, hapticFeedbackFrequency);
                if(secondaryGripPickup)
                {
                    if(secondaryGripPickup.IsHeld)
                    {
                        //get the opposite hand to play a haptic event
                        if(hand == VRC_Pickup.PickupHand.Left)
                        {
                            Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, hapticFeedbackDuration, hapticFeedbackAmplitude, hapticFeedbackFrequency);
                        }
                        else
                        {
                            Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, hapticFeedbackDuration, hapticFeedbackAmplitude, hapticFeedbackFrequency);
                        }
                    }
                }
            }
            if (targetRigidbody && localPlayer == Networking.GetOwner(targetRigidbody.gameObject))
            {
                targetRigidbody.AddForceAtPosition((-firePosition.forward) * forceFromBarrel, firePosition.position);
                Debug.Log("force applied, " + ((-firePosition.forward) * forceFromBarrel).magnitude);
            }
            if (RaycastDamage)
            {

                //raycast stuff
                if (shotgun)
                {
                    for (int i = 0; i <= pelletCount; i++)
                    {
                        if (desktopFaceFiring && DesktopUser && Networking.IsOwner(gameObject))
                        {
                            firePosition.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                            firePosition.rotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
                        }
                        Quaternion temp = firePosition.rotation;
                        firePosition.Rotate(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));
                        if(gunShotParticle)
                        {
                            gunShotParticle.Play();
                        }
                        BulletRaycast();
                        
                        firePosition.rotation = temp;
                    }
                    if (!finalCycleAnimation && GunAnimator && CycleAnimation) //normal fire
                    {
                        GunAnimator.Play("Base Layer." + CycleAnimation.name, 0, 0);
                    }

                    if (finalCycleAnimation && GunAnimator && CycleAnimation && AmmoCount >= 2) //bolt action when more than 1 bullet
                    {
                        GunAnimator.Play("Base Layer." + CycleAnimation.name, 0, 0);
                    }
                    if (GunAnimator && finalCycleAnimation && AmmoCount <= 1) //bolt action on last bullet
                    {
                        GunAnimator.Play("Base Layer." + finalCycleAnimation.name, 0, 0);
                    }

                    AmmoCount--;
                    if (!finalGunShotClip && gunShot != null && barrelAudioSource)
                    {
                        if (randomGunShotClip.Length == 0)
                        {
                            Debug.LogWarning("No audio clips assigned to RandomAudioClipPlayer.");
                            barrelAudioSource.PlayOneShot(gunShot);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }
                    if (finalGunShotClip && gunShot != null && barrelAudioSource && AmmoCount >= 1)
                    {
                        if (randomGunShotClip.Length == 0)
                        {
                            Debug.LogWarning("No audio clips assigned to RandomAudioClipPlayer.");
                            barrelAudioSource.PlayOneShot(gunShot);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }
                    if (finalGunShotClip != null && barrelAudioSource && AmmoCount <= 0 && randomGunShotClip.Length == 0)
                    {
                        barrelAudioSource.PlayOneShot(finalGunShotClip);
                    }
                    if (randomGunShotClip.Length > 1 && barrelAudioSource && !gunShot)
                    {
                        if (AmmoCount <= 0 && finalGunShotClip)
                        {
                            barrelAudioSource.PlayOneShot(finalGunShotClip);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }


                    if (ShellParticle != null)
                    {
                        ShellParticle.Play();
                    }
                    
                }
                else
                {

                    if (desktopFaceFiring && DesktopUser && Networking.IsOwner(gameObject))
                    {
                        firePosition.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                        firePosition.rotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
                    }
                    Quaternion temp = firePosition.localRotation;
                    firePosition.Rotate(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));
                    BulletRaycast();
                    firePosition.localRotation = temp;

                    if (!finalCycleAnimation && GunAnimator && CycleAnimation) //normal fire/non bolt action
                    {
                        GunAnimator.Play("Base Layer." + CycleAnimation.name, 0, 0);
                    }

                    if (finalCycleAnimation && GunAnimator && CycleAnimation && AmmoCount >= 2) //bolt action when more than 1 bullet
                    {
                        GunAnimator.Play("Base Layer." + CycleAnimation.name, 0, 0);
                    }
                    if (finalCycleAnimation && GunAnimator && AmmoCount <= 1) //bolt action on last bullet
                    {
                        GunAnimator.Play("Base Layer." + finalCycleAnimation.name, 0, 0);
                    }

                    AmmoCount--;
                    if (!finalGunShotClip && gunShot != null && barrelAudioSource) //only if non bolt action
                    {
                        if (randomGunShotClip.Length == 0)
                        {
                            Debug.LogWarning("No audio clips assigned to RandomAudioClipPlayer.");
                            barrelAudioSource.PlayOneShot(gunShot);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }
                    if (finalGunShotClip && gunShot != null && barrelAudioSource && AmmoCount >= 1) //normal but bolt
                    {
                        if (randomGunShotClip.Length == 0)
                        {
                            Debug.LogWarning("No audio clips assigned to RandomAudioClipPlayer.");
                            barrelAudioSource.PlayOneShot(gunShot);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }
                    if (finalGunShotClip != null && barrelAudioSource && AmmoCount <= 0 && randomGunShotClip.Length == 0)
                    {
                        barrelAudioSource.PlayOneShot(finalGunShotClip);
                    }
                    if (randomGunShotClip.Length > 1 && barrelAudioSource && !gunShot)
                    {
                        if (AmmoCount <= 0 && finalGunShotClip)
                        {
                            barrelAudioSource.PlayOneShot(finalGunShotClip);
                        }
                        else
                        {
                            int newClipIndex;
                            do
                            {
                                newClipIndex = Random.Range(0, randomGunShotClip.Length);
                            } while (newClipIndex == lastClipIndex);

                            lastClipIndex = newClipIndex;
                            barrelAudioSource.PlayOneShot(randomGunShotClip[newClipIndex]);
                        }
                    }

                    if (ShellParticle != null)
                    {
                        ShellParticle.Play();
                    }
                    if (gunShotParticle)
                    {
                        gunShotParticle.Play();
                    }
                }
            }
            else
            {

                //projectile stuff
                if (shotgun == false)
                {

                    //bullet shooting
                    //TODO replace instantiation with localised object pools

                    var bul = VRCInstantiate(bullet);



                    bul.transform.SetParent(null);
                    bul.SetActive(true);
                    if (desktopFaceFiring && DesktopUser && Networking.IsOwner(gameObject))
                    {
                        bul.transform.SetPositionAndRotation(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position, localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation);
                    }
                    else
                    {
                        bul.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                    }


                    bul.transform.Rotate(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));

                    var bulletrb = (Rigidbody)bul.GetComponent(typeof(Rigidbody));
                    bulletrb.velocity = (bul.transform.forward) * fireVelocity;
                    if (GunAnimator != null)
                    {
                        GunAnimator.Play("Base Layer." + AnimName, 0, 0);
                    }

                    AmmoCount--;
                    if (gunShot != null && barrelAudioSource)
                    {
                        barrelAudioSource.PlayOneShot(gunShot);
                    }   

                    if (ShellParticle!=null)
                    {
                        ShellParticle.Play();
                    }

                }
                else
                {
                    //shotgun

                    for (int i = 0; i <= pelletCount; i++)
                    {
                        var bul = VRCInstantiate(bullet);
                        bul.transform.parent = null;

                        bul.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                        bul.transform.Rotate(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));
                        bul.SetActive(true);
                        var bulletrb = (Rigidbody)bul.GetComponent(typeof(Rigidbody));
                        bulletrb.velocity = (bul.transform.forward) * fireVelocity;
                    }
                    if (GunAnimator != null)
                    {
                        GunAnimator.Play("Base Layer." + AnimName, 0, 0);
                    }

                    AmmoCount--;
                    if (gunShot != null && barrelAudioSource)
                    {
                        barrelAudioSource.PlayOneShot(gunShot);
                    }                

                    if (ShellParticle!=null)
                    {
                        ShellParticle.Play();
                    }
                }
            }
        }
        else
        {
            //ammocount is empty
            bulletInChamber = false;
            if(reloadOnMagEmpty && MaxAmmo > 0)
            {
                Reload();
            }
        }
        //Debug.Log("Ammo Left: " + AmmoCount);
    }

    private void HandleGunRotation()
    {
        float actualShoulderActivationDistance = relativeVirtualStockActivationDistance * playerHeight;
        //check if the stocktransform is near one of the shoulders, using the tracking data for the shoulders
        
        
        
    }

    public void Reload()
    {
        reloading = true;
        if (AmmoCount > 0)
        {
            bulletInChamber = true;
        }
        if (Display != null)
        {
            Display.text = "Reloading";
        }

        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetFireFalse");
        if (MagPull != null && magazineAudioSource)
        {
            if (!reloadAudioBypass)
            {
                magazineAudioSource.PlayOneShot(MagPull);
            }
            //magazineAudioSource.PlayOneShot(MagPull);
        }
        

        if(ReloadAnimation && GunAnimator && !perShellReload && !finalReloadAnimation)
        {
            reloadTime = ReloadAnimation.length - reloadAnimationOffset; //takes time off reload time
            GunAnimator.Play("Base Layer." + ReloadAnimation.name, 0, 0);   
        }
        if (finalReloadAnimation && GunAnimator && AmmoCount == 0 && !perShellReload) //plays seperate animation for reloading when no ammo
        {
            reloadTime = ReloadAnimation.length - finalReloadAnimationOffset;//takes time off reload time
            GunAnimator.Play("Base Layer." + finalReloadAnimation.name, 0, 0);
        }
        if (finalReloadAnimation && GunAnimator && AmmoCount >= 1 && !perShellReload) //plays seperate animation for reloading when low ammo
        {
            reloadTime = ReloadAnimation.length - reloadAnimationOffset;//takes time off reload time
            GunAnimator.Play("Base Layer." + ReloadAnimation.name, 0, 0);
        }

        if (ReloadAnimation && GunAnimator && perShellReload)
        {
                   switch(MagSize - AmmoCount)
            {
                    case 0: //full reload
                    //this is where a full reload would go if i knew how to make one.
                    break;
                    case 1: //only 1 shell
                    reloadTime = ReloadAnimation1.length - reloadAnimation1Offset;//takes time off reload time
                    GunAnimator.Play("Base Layer." + ReloadAnimation1.name, 0, 0);


                    break;
                    case 2: //only 2 shells
                    reloadTime = ReloadAnimation2.length - reloadAnimation2Offset;//takes time off reload time
                    GunAnimator.Play("Base Layer." + ReloadAnimation2.name, 0, 0);


                    break;
                    case 3: //only 3 shells
                    reloadTime = ReloadAnimation3.length - reloadAnimation3Offset;//takes time off reload time
                    GunAnimator.Play("Base Layer." + ReloadAnimation3.name, 0, 0);


                    break;
                    case 4: //you get the point
                    reloadTime = ReloadAnimation4.length - reloadAnimation4Offset;//takes time off reload time
                    GunAnimator.Play("Base Layer." + ReloadAnimation4.name, 0, 0);


                    break;
                    case 5:
                    reloadTime = ReloadAnimation5.length - reloadAnimation5Offset;//takes time off reload time
                    GunAnimator.Play("Base Layer." + ReloadAnimation5.name, 0, 0);


                    break;
                    case 6:
                    reloadTime = ReloadAnimation6.length - reloadAnimation6Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation6.name, 0, 0);


                    break;
                    case 7:
                    reloadTime = ReloadAnimation7.length - reloadAnimation7Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation7.name, 0, 0);


                    break;
                    case 8:
                    reloadTime = ReloadAnimation8.length - reloadAnimation8Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation8.name, 0, 0);


                    break;
                    case 9:
                    reloadTime = ReloadAnimation9.length - reloadAnimation9Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation9.name, 0, 0);


                    break;
                    case 10:
                    reloadTime = ReloadAnimation10.length - reloadAnimation10Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation10.name, 0, 0);


                    break;
                    case 11:
                    reloadTime = ReloadAnimation11.length - reloadAnimation11Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation11.name, 0, 0);


                    break;
                    case 12: //all 12 shells reload
                    reloadTime = ReloadAnimation12.length - reloadAnimation12Offset;
                    GunAnimator.Play("Base Layer." + ReloadAnimation12.name, 0, 0);

                    break;
                    default: break;

            }
        }


        Debug.Log("Reloading");
        
        AdjustAudioSourceTiming();
        
        //set the reload audio timers
        if (MagInsert != null)
        {
            magInsertTimer = MagInsert.length;
            if(GunCock != null)
            {
                magInsertTimeStamp = Time.time + reloadTime - MagInsert.length - GunCock.length;
            }
        }
        if (GunCock != null)
        {
            gunCockTimer = GunCock.length;
        }
        if (MagPull != null)
        {
            magPullTimer = MagPull.length;
        }
        //set the reload bools to false
        magPulled = false;
        magInserted = false;
        gunCocked = false;
        //play the mag pull sound first
        if (MagPull != null && magazineAudioSource)
        {
            if (!reloadAudioBypass)
            {
                magazineAudioSource.PlayOneShot(MagPull);
            }
            //magazineAudioSource.PlayOneShot(MagPull);
            Debug.Log("MagPull audio");
        }
        //send changes to network
        if (localPlayer.IsOwner(gameObject))
        {
            RequestSerialization();
        }
    }

    private void AdjustAudioSourceTiming()
    {
        //add up the durations of the reloading audio clips, and compare it to the reload time
        float reloadAudioDuration = 0;
        if (MagPull != null)
        {
            reloadAudioDuration += MagPull.length;
        }
        if (MagInsert != null)
        {
            reloadAudioDuration += MagInsert.length;
        }
        if (GunCock != null)
        {
            reloadAudioDuration += GunCock.length;
        }

        if (reloadAudioDuration > reloadTime)
        {
            //if the audio is longer than the reload time, then we need to speed up the audio
            float speedUpFactor = reloadAudioDuration / reloadTime;
            if (MagPull != null)
            {
                MagPull = AudioClip.Create(MagPull.name, (int)(MagPull.samples / speedUpFactor), MagPull.channels, MagPull.frequency, false);
            }
            if (MagInsert != null)
            {
                MagInsert = AudioClip.Create(MagInsert.name, (int)(MagInsert.samples / speedUpFactor), MagInsert.channels, MagInsert.frequency, false);
            }
            if (GunCock != null)
            {
                GunCock = AudioClip.Create(GunCock.name, (int)(GunCock.samples / speedUpFactor), GunCock.channels, GunCock.frequency, false);
            }
        }
        else
        {
            //ensure the audio clips are their default length
            
        }
    }
}
