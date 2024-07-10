using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class SkinManager : MonoBehaviour
{
    // ~~ 1. Updates All Animations to Match Player Selections

    [SerializeField] private SO_CharacterBody characterBody;

    // String Arrays
    [SerializeField] private string[] bodyPartTypes;
    [SerializeField] private string[] characterStates;
    [SerializeField] private string[] characterDirections;

    // Animation
    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    [HideInInspector] public PhotonView photonView;
    
    private void Start()
    {
        // Set animator
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(defaultAnimationClips);

        photonView = GetComponent<PhotonView>();
        // Set body part animations
        UpdateBodyParts();
    }

    public void UpdateBodyParts()
    {
        // Override default animation clips with character body parts
        for (int partIndex = 0; partIndex < bodyPartTypes.Length; partIndex++)
        {
            // Get current body part
            string partType = bodyPartTypes[partIndex];
            // Get current body part ID
            string partID = characterBody.characterBodyParts[partIndex].bodyPart.bodyPartAnimationID.ToString();

            for (int stateIndex = 0; stateIndex < characterStates.Length; stateIndex++)
            {
                string state = characterStates[stateIndex];
                for (int directionIndex = 0; directionIndex < characterDirections.Length; directionIndex++)
                {
                    string direction = characterDirections[directionIndex];
                    // Get players animation from player body
                    // ***NOTE: Unless Changed Here, Animation Naming Must Be: "[Type]_[Index]_[state]_[direction]" (Ex. Body_0_idle_down)
                    animationClip = Resources.Load<AnimationClip>("Animations/" + "Player/" + partType + "/" + partType + partID + "_" + state + "_" + direction);

                    // Override default animation
                    defaultAnimationClips[partType + 0 + "_" + state + "_" + direction] = animationClip;
                }
            }
        }

        if (photonView.IsMine)
        {
            // Apply updated animations
            animatorOverrideController.ApplyOverrides(defaultAnimationClips);
        }

        SyncAnimationClips();
    }

    public void SyncAnimationClips()
    {
        if (photonView.IsMine)
        {
            // Convert your animation clips info to a format that can be sent over the network
            List<string> clipNames = new List<string>();
            List<string> clipPaths = new List<string>();

            for (int partIndex = 0; partIndex < bodyPartTypes.Length; partIndex++)
            {
                string partType = bodyPartTypes[partIndex];
                string partID = characterBody.characterBodyParts[partIndex].bodyPart.bodyPartAnimationID.ToString();
                for (int stateIndex = 0; stateIndex < characterStates.Length; stateIndex++)
                {
                    string state = characterStates[stateIndex];
                    for (int directionIndex = 0; directionIndex < characterDirections.Length; directionIndex++)
                    {
                        string direction = characterDirections[directionIndex];
                        string clipName = partType + 0 + "_" + state + "_" + direction;
                        string clipPath = @"Asset/Resourses/Animations/Player/" + partType + "/" + partType + partID + "_" + state + "_" + direction;
                        Debug.Log(clipPath);
                        clipNames.Add(clipName);
                        clipPaths.Add(clipPath);
                    }
                }
            }

            photonView.RPC("RPC_UpdateAnimationClips", RpcTarget.All, clipNames.ToArray(), clipPaths.ToArray());
        }
    }
    [PunRPC]
    public void RPC_UpdateAnimationClips(string[] clipNames, string[] clipPaths)
    {
        for (int i = 0; i < clipNames.Length; i++)
        {
            AnimationClip clip = Resources.Load<AnimationClip>(clipPaths[i]);
            defaultAnimationClips[clipNames[i]] = clip;
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }



    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}

//using System.Collections.Generic;
//using Photon.Pun;
//using UnityEngine;

//public class SkinManager : MonoBehaviourPun, IPunObservable
//{
//    [SerializeField] private SO_CharacterBody characterBody;
//    [SerializeField] private string[] bodyPartTypes;
//    [SerializeField] private string[] characterStates;
//    [SerializeField] private string[] characterDirections;

//    private Animator animator;
//    private AnimationClip animationClip;
//    private AnimatorOverrideController animatorOverrideController;
//    private AnimationClipOverrides defaultAnimationClips;

//    private void Start()
//    {
//        animator = GetComponent<Animator>();
//        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
//        animator.runtimeAnimatorController = animatorOverrideController;

//        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
//        animatorOverrideController.GetOverrides(defaultAnimationClips);

//        if (photonView.IsMine)
//        {
//            UpdateBodyParts();
//        }
//    }

//    public void UpdateBodyParts()
//    {
//        if (!photonView.IsMine) return;

//        Debug.Log("Updating body parts for local player");

//        for (int partIndex = 0; partIndex < bodyPartTypes.Length; partIndex++)
//        {
//            string partType = bodyPartTypes[partIndex];
//            string partID = characterBody.characterBodyParts[partIndex].bodyPart.bodyPartAnimationID.ToString();

//            for (int stateIndex = 0; stateIndex < characterStates.Length; stateIndex++)
//            {
//                string state = characterStates[stateIndex];
//                for (int directionIndex = 0; directionIndex < characterDirections.Length; directionIndex++)
//                {
//                    string direction = characterDirections[directionIndex];
//                    string animationPath = "Animations/Player/" + partType + "/" + partType + partID + "_" + state + "_" + direction;
//                    Debug.Log("Loading animation from path: " + animationPath);
//                    animationClip = Resources.Load<AnimationClip>(animationPath);

//                    if (animationClip == null)
//                    {
//                        Debug.LogError("Animation clip not found at path: " + animationPath);
//                    }

//                    defaultAnimationClips[partType + 0 + "_" + state + "_" + direction] = animationClip;
//                }
//            }
//        }

//        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
//        photonView.RPC("SyncAnimations", RpcTarget.Others, bodyPartTypes, characterStates, characterDirections);
//    }

//    [PunRPC]
//    private void SyncAnimations(string[] bodyPartTypes, string[] characterStates, string[] characterDirections)
//    {
//        this.bodyPartTypes = bodyPartTypes;
//        this.characterStates = characterStates;
//        this.characterDirections = characterDirections;

//        UpdateBodyParts();
//    }

//    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
//    {
//        public AnimationClipOverrides(int capacity) : base(capacity) { }

//        public AnimationClip this[string name]
//        {
//            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
//            set
//            {
//                int index = this.FindIndex(x => x.Key.name.Equals(name));
//                if (index != -1)
//                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
//            }
//        }
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext(bodyPartTypes);
//            stream.SendNext(characterStates);
//            stream.SendNext(characterDirections);
//        }
//        else
//        {
//            bodyPartTypes = (string[])stream.ReceiveNext();
//            characterStates = (string[])stream.ReceiveNext();
//            characterDirections = (string[])stream.ReceiveNext();

//            UpdateBodyParts();
//        }
//    }
//}

