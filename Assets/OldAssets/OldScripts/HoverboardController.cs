using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//the main script for the character controller, but also a few additional game bits attached such as the players name and customisation choices from the menu.

public class HoverboardController : MonoBehaviour
{
    public string playerName = ("Cringe Cat");

    public bool controlsDisabled = false;

    public GameObject rayPoint;

    [SerializeField]
    public string playerPrefix = "";

    private Rigidbody body;
    private float deadZone = 0.1f;
    [HideInInspector]
    public float thrust = 0.0f;
    public LayerMask layerMask;

    [HideInInspector]
    public bool wipedOut = false;
    
    public Rigidbody[] ragdollParts;
    public GameObject riderModel;
    public Animator charAnim;

    [SerializeField]
    private float groundedDrag = 3.0f;
    [SerializeField]
    private float gravityForce = 0.982f;
    [SerializeField]
    private float hoverHeight = 1.5f;


    public float collisionThreshold = 500f;

    //control variables.
    [SerializeField]
    private float forwardAcceleration = 16.0f;
    [SerializeField]
    private float reverseAcceleration = 8.0f;
    [SerializeField]
    private float turnSpeed = 5.0f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 2f;
    [SerializeField]
    private float slopeSensitivity = 5f;

    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool aerial;
    [HideInInspector]
    public bool grindHopping = false;

    public Transform riderTransform;


    [HideInInspector]
    public float velocity;

    [SerializeField]
    private PlayerSounds playerSounds; //Reference to the player sounds script

    public GameObject landingParticle;
    private bool hasLanded = false;
    
    //variables relevant to the grinding system
    [HideInInspector]
    public bool canGrind = true;

    private float grindCoolDownTimer = 0f;
    private float grindCoolDown = 1f;

    public GameObject grindEffect;
    [HideInInspector]
    public List<Transform> nodesToGrind = new List<Transform>();
    [HideInInspector]
    public bool grinding = false;
    [HideInInspector]
    public Transform currentGrindTarget = null;
    int grindIndex = 0;
    [HideInInspector]
    public Vector3 LastVelocity = new Vector3();
    [HideInInspector]
    public Transform lastCheckpoint;
    [HideInInspector]
    public bool isTricking = false;
    private int trickCount = 0;

    //score ui
    public Text radicalText;
    public Text cringeText;

    [SerializeField]
    private float maxTimeBetweenTricks = 1f;
    //trick variables
    [HideInInspector]
    public int trickCounter;
    private float lastTrickTime;
    private int multiplier = 1;
    public Text multiplierText;
    public Animator comboAnim;
    private int currentComboScore;
    public Text currentComboText;


    private FMOD.Studio.EventInstance instance;

    public int radicalScore = 0;
    public int cringeScore = 0;

    //all the customisation options that the player can select from
    public GameObject catMesh;
    public Material[] skins;
    public GameObject[] hats;
    public GameObject[] boards;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        body.centerOfMass = Vector3.down;

        layerMask = ~layerMask;
        //get our players name from the menu, if we don't have one default to mysterious skater.
        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerName == ("")){
            playerName = "Mysterious Skater";
        }
        //if a controller is plugged in, use the controller, if not we go with keyboard.
        playerPrefix = PlayerPrefs.GetString("ControllerPref");
        if (Input.GetJoystickNames().Length > 0)
        {
            playerPrefix = "P1";
        }

        //pick the customisation options that got picked in the menu. the list of options must match the list in the menu, so this definitely needs improvement.
        hats[PlayerPrefs.GetInt("CurrentHat")].SetActive(true);
        boards[PlayerPrefs.GetInt("CurrentBoard")].SetActive(true);
        catMesh.GetComponent<SkinnedMeshRenderer>().material = skins[PlayerPrefs.GetInt("CurrentSkin")];


        
    }

    private void Update()
    {

        //update our score ui text
        radicalText.text = (radicalScore.ToString());
        cringeText.text = (cringeScore.ToString());


        if (!controlsDisabled)
        {
            //reset key, if we are not dead, die, if we are dead, respawn.
            if (Input.GetButtonDown(playerPrefix + "Reset"))
            {

                radicalText.text = (radicalScore.ToString());
                cringeText.text = (cringeScore.ToString());
                if (!wipedOut)
                {
                    WipeOut();
                }
                else if (wipedOut)
                {
                    Debug.Log("RESET");
                    Reset(lastCheckpoint);
                }

            }

            if (!wipedOut)
            {
                playerSounds.isAlive(); // if the player is alive then the music is set to normal values
                if (grinding)
                {
                    if (isTricking)
                    {
                        //if we try to grind while flipping the board, wipe out
                        WipeOut();
                    }

                    //once we have recieved a grind,  set our position to the current grind target (the next node) over time
                    transform.position = Vector3.MoveTowards(transform.position, currentGrindTarget.position, 25 * Time.deltaTime);
                    transform.LookAt(currentGrindTarget);
                    //rotate the characte along the rail

                    //once we have passed a node, set our next target as the next node, or bump us off the rail if no more nodes.
                    float distanceToTarget = Vector3.Distance(currentGrindTarget.position, transform.position);
                    if (distanceToTarget < 1.0)
                    {
                        if (grindIndex + 1 < nodesToGrind.Count)
                        {
                            grindIndex += 1;
                            currentGrindTarget = nodesToGrind[grindIndex];
                        }
                        else
                        {
                            ExitGrind();
                        }

                    }
                }

                else
                {
                    //if we aren't grinding, player has control as normal
                    if (grounded)
                    {
                        aerial = false;
                        charAnim.SetBool("Aerial", false);
                        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        instance.release();

                    }

                    thrust = 0.0f;

                    float acceleration = Input.GetAxis(playerPrefix + "Vertical");

                    if (acceleration > deadZone)
                    {
                        thrust = acceleration * forwardAcceleration;

                        //playerSounds.accelerate();// Reference for acceleration             
                    }
                    else if (acceleration < -deadZone)
                    {
                        thrust = acceleration * reverseAcceleration;

                        //playerSounds.decelerate();// Reference for deceleration
                    }

                    else thrust = 50f;




                }



            }
            else
            {
                playerSounds.isDead(); //if the player is wiped out then it calls for the parameter to be set to distort


                playerSounds.resetScoreSound();
            }


            if (grinding)// References when the player is on/off the rail
            {
                playerSounds.StartGrindSound();
            }
            else
            {
                playerSounds.StopGrindSound();
            }
        }
 

    }


    // on a collision, wipe the player out if we hit something hard enough
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (collisionForce < collisionThreshold)
        {
            //no crash
        }
        else
        {
            if (!wipedOut)
            {
                WipeOut();
            }

        }
    }




    private void FixedUpdate()
    {


        if (!grinding)
        {

            
            //cast a ray down from the bottom of the board, to see where we are at
            RaycastHit hit;
            grounded = false;
            LastVelocity = body.velocity;

            if (Physics.Raycast(rayPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask))
            {
                grounded = true;
                if (isTricking == true)
                {//if we try to trick but hit the ground, die
                    if (!wipedOut){
                        WipeOut();
                    }

                }



                //once we land, create our particle effect, and if we finished a combo, rack up the score.
                if (hasLanded == false)
                {
                    GameObject landing = Instantiate(landingParticle, gameObject.transform.position,Quaternion.Euler(90,0,0));
                    hasLanded = true;
                    charAnim.SetTrigger("Landed");
                    if (trickCount > 0)
                    {
                        //Add to combo
                        Debug.Log("LANDED TRICK");
                        LandTrick(trickCount);
                        trickCount = 0;
                    }
                    charAnim.SetBool("JumpGo", false);
                    Destroy(landing, 1f);
                }


                playerSounds.playLandSound();

                //move the board up from the ground, based on the distance our raycast has and our desired distance from the ground
                float correction = (hoverHeight - hit.distance);
                Vector3 targetVector = new Vector3(transform.position.x, transform.position.y + correction, transform.position.z);
                body.MovePosition(targetVector);

                //a really janky solution for going faster down hills without relying on unity physics,
                //use a shpere cast to get the angle of a slope, and then adjust our drag value based on the slope.
                //this really sucks and needs to be improved.
                if (Physics.SphereCast(transform.position, .25f, Vector3.down, out hit, 3f))
                {
                    float slope = Vector3.Dot(transform.right, (Vector3.Cross(Vector3.up, hit.normal)));
                    groundedDrag = (3.0f - (slope * slopeSensitivity));

                }
            }
            else
            {
                //if we aren't grounded we are aerial.
                grounded = false;
                canGrind = true;
                //body.AddForce(Vector3.down * gravityForce * body.mass);

                //only go fully aerial if we are twice the height of our desired height from the ground, to prevent mini aerials from going down janky slopes or curbs.
                //this is fine for a hoverboard, but not necessary for a skateboard.
                if (!Physics.Raycast(rayPoint.transform.position, -Vector3.up, out hit, hoverHeight * 2, layerMask))
                {
                    aerial = true;
                    charAnim.SetBool("Aerial", true);
                    hasLanded = false;

                }
            }

            if (grounded)
            {
                body.drag = groundedDrag;
            }
            else
            {
                //if we are aerial give the player some extra speed to transition grinds.
                body.drag = 0.05f;
                thrust /= 5f;
            }

            if (Mathf.Abs(thrust) > 0)
            {
                //apply thrust based on input
                body.AddForce(transform.forward * thrust * body.mass);
            }

            //better jump?
            //i found this jump bit online, for making a mario curve jump, you jump up higher faster than you fall down, to make the jump feel snappier.
            //not completely sure how it works
            if (body.velocity.y < 0)
            {
                body.velocity += Vector3.up * -gravityForce * (fallMultiplier) * Time.deltaTime;

            }
            else if(body.velocity.y > 0){

                body.velocity += Vector3.up * -gravityForce * (lowJumpMultiplier) * Time.deltaTime;
            }

        }

        if (!controlsDisabled)
        {
            if (!grinding)
            {
                //if we aren't grinding and can use the controls, rotate based on input
                transform.Rotate(0.0f, -Input.GetAxis(playerPrefix + "Horizontal") * turnSpeed, 0.0f);
                charAnim.SetFloat("HorizontalAxis", -Input.GetAxis(playerPrefix + "Horizontal"), 0.05f, Time.deltaTime);
            }

            //if we hold the jump button, the player crouches down for an ollie, once we let go the jump is triggered.
            if (Input.GetButtonDown(playerPrefix + "Jump"))
            {
                charAnim.SetBool("JumpPrep", true);
            }

            if (Input.GetButtonUp(playerPrefix + "Jump"))
            {
                charAnim.SetBool("JumpGo", true);
                charAnim.SetBool("JumpPrep", false);
                if (grinding)
                {

                    GrindHop();
                }
                else if (grounded)
                {
                    Jump(jumpForce);
                }


            }

            //trigger a trick if we press our button and we are in the air and not already tricking or dead
            if ((Input.GetButtonDown(playerPrefix + "Trick")) && aerial && !grinding && !isTricking && !wipedOut)
            {
                //get a random trick animation, add score, and update the trick text at the bottom
                Random.InitState((int)System.DateTime.Now.Ticks);
                int randomNumber = Random.Range(1, 6);
                charAnim.SetTrigger("Trick" + randomNumber);
                isTricking = true;
                trickCount++;
                comboAnim.SetTrigger("Trick");
                multiplier++;
                multiplierText.text = ("x" + multiplier);
                currentComboScore += 50;
                currentComboText.text = ("" + currentComboScore);

                playerSounds.playScoreSound();

            }

            if (!canGrind)
            {   // a small cooldown on grinding, so that you don't get sucked back onto the same rail once exiting.
                grindCoolDownTimer -= Time.deltaTime;
                if (grindCoolDownTimer <= 0f)
                {
                    canGrind = true;

                }

            }
        }


    }

    //two different jump types for exiting a rail and a regular flat ground jump
    //this is because the rail jump needed some extra power to be able to transition between the rails either side of the road
    //the rail jump is also launched in the direction that you input.
    public void RailJump(float ForwardPower, float HorizPower, float UpPower)
    {
        //rail jump from the rail based on the direction of input.
        float h = Input.GetAxisRaw(playerPrefix + "Horizontal");
        //float v = Input.GetAxisRaw(playerPrefix + "Vertical");
        Vector3 JumpVector = (transform.forward * (ForwardPower * body.mass)) + (transform.right * h * (HorizPower * body.mass)) + (transform.up * UpPower * body.mass);
        body.AddForce(JumpVector, ForceMode.Impulse);
        grindHopping = true;
        Debug.Log("RAIL JUMP");
    }

    // a regular flat ground jump
    public void Jump(float power)
    {
        playerSounds.playJumpSound();

        playerSounds.set_inAir(true);

        body.AddForce(transform.up * power * body.mass, ForceMode.Impulse);
        Debug.Log("REG JUMP");
    }


    //A controlled exit, when the player comes off the grind

    public void ExitGrind()
    {
        //if we detach from a rail, reset the list of nodes, set our player back to non kinematic, add a trick, and reset animation.
        grinding = false;
        grindIndex = 0;
        body.isKinematic = false;
        //LastVelocity.magnitude
        body.velocity = (currentGrindTarget.transform.forward * jumpForce);     
        nodesToGrind.Clear();
        currentGrindTarget = null;
        grindEffect.SetActive(false);
        transform.rotation = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0);
        trickCount++;
        comboAnim.SetTrigger("Trick");
        multiplier++;
        multiplierText.text = ("x" + multiplier);
        currentComboScore += 50;
        currentComboText.text = ("" + currentComboScore);

        playerSounds.playScoreSound();

        charAnim.SetBool("Grinding1", false);
        charAnim.SetBool("Grinding2", false);
        charAnim.SetBool("Grinding3", false);
        charAnim.SetBool("Grinding4", false);
        charAnim.SetBool("Grinding5", false);
    }

    //If the player hits the jump while on a rail we exit the rail and make a small hop.
    public void GrindHop()
    {
        canGrind = false;
        grindCoolDownTimer = grindCoolDown;
        //raise the player by a unit
        gameObject.transform.position += Vector3.up * 2f;
        //body.MovePosition(transform.position + Vector3.up * 2f);
        grinding = false;
        grindIndex = 0;
        body.isKinematic = false;
        nodesToGrind.Clear();
        currentGrindTarget = null;
        grindEffect.SetActive(false);
        transform.rotation = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0);
        if (playerPrefix == "P1"){
            RailJump(jumpForce, jumpForce / 1.5f, jumpForce / 2);
        }
        else
        {
            RailJump(jumpForce, jumpForce / 2, jumpForce / 2);
        }

        trickCount++;
        comboAnim.SetTrigger("Trick");
        multiplier++;
        multiplierText.text = ("x" + multiplier);
        playerSounds.playScoreSound();
        currentComboScore += 50;
        currentComboText.text = ("" + currentComboScore);
        charAnim.SetBool("Grinding1", false);
        charAnim.SetBool("Grinding2", false);
        charAnim.SetBool("Grinding3", false);
        charAnim.SetBool("Grinding4", false);
        charAnim.SetBool("Grinding5", false);

    }

    public void WipeOut()
    {

        //set all skeleton rigidbodies to non kinematic, disable controls, add a wipeout, and detach the player character from the board.
        foreach (Rigidbody part in ragdollParts)
        {
            part.isKinematic = false;

        }
        charAnim.SetBool("Dead", true);
        charAnim.enabled = false;
        wipedOut = true;
        riderModel.transform.parent = null;
        float penalty = radicalScore * 0.2f;
        radicalScore -= Mathf.RoundToInt(penalty);
        cringeScore += 1;
        trickCount = 0;
        multiplier = 1;
        currentComboScore = 0;
        currentComboText.text = "";
        multiplierText.text = "";
    }

    // reset the player post wipeout if we press the reset key
    public void Reset(Transform respawnPos)
    {
       //if the player was grinding, detach from the grind rail, reset the nodes and reset transform.
        if (grinding)
        {
            grinding = false;
            grindIndex = 0;
            body.isKinematic = false;
            nodesToGrind.Clear();
            currentGrindTarget = null;
            grindEffect.SetActive(false);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        //stick the player back to the respawn (taken from a checkpoint), renable physics, input, and velocity
        transform.position = respawnPos.position;
        transform.rotation = Quaternion.Euler(0, respawnPos.eulerAngles.y, 0);
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        riderModel.transform.parent = transform;

        foreach (Rigidbody part in ragdollParts)
        {
            part.isKinematic = true;

        }
        isTricking = false;
        wipedOut = false;
        charAnim.enabled = true;
        charAnim.SetBool("Dead", false);
        charAnim.SetBool("Grinding1", false);
        charAnim.SetBool("Grinding2", false);
        charAnim.SetBool("Grinding3", false);
        charAnim.SetBool("Grinding4", false);
        charAnim.SetBool("Grinding5", false);
        //reattach the player to the board and reset anims
        riderModel.transform.position = riderTransform.position;
        riderModel.transform.rotation = riderTransform.rotation;
    }

    //if we land safely, bank our score points and reset the multiplier. wipe the score text.
    public void LandTrick(int trickCount)
    {

        trickCounter += trickCount;
        radicalScore += 50 * trickCounter * multiplier;
        trickCounter = 0;
        currentComboScore = 0;
        currentComboText.text = "";
        multiplier = 1;
        multiplierText.text = "";


        playerSounds.resetScoreSound();
    }
}
