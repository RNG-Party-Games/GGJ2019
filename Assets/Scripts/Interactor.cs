using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Interactor : MonoBehaviour
{

    public int value = 1;
    public bool active = true;
    public enum InteractorType {Collectible, TV, KotatsuPlugged, KotatsuUnplugged, Door, Lamp, Phone};
    public InteractorType type = InteractorType.Collectible;
    public Material textureSwap;
    Material originalTexture;
    public GameObject objectSwap;
    public string animation;
    public PostProcessVolume volume;
    public Color lightsOff;
    Color lightsOn = new Color(1,1,1,1);
    public AudioClip clip;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Outline>().enabled = false;
        if(type == InteractorType.TV) {
            originalTexture = GetComponent<MeshRenderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if(active && collider.tag == "InteractorField") {
            collider.gameObject.transform.parent.GetComponent<Character>().AddInteractor(this);
        }
    }

    void OnTriggerExit(Collider collider) {
        if(collider.tag == "InteractorField") {
            collider.gameObject.transform.parent.GetComponent<Character>().RemoveInteractor(this);
        }
    }

    public int GetValue() {
        return value;
    }

    public void Select() {
        GetComponent<Outline>().enabled = true;
    }

    public void DeSelect() {
        GetComponent<Outline>().enabled = false;
    }

    public void Collect() {
        if(type == InteractorType.Collectible) {
            active = false;
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.KotatsuPlugged || type == InteractorType.KotatsuUnplugged) {
            objectSwap.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.Door) {
            active = false;
            GetComponent<Animator>().Play(animation);
        }
        else if(type == InteractorType.TV) {
            active = false;
            GetComponent<MeshRenderer>().material = textureSwap;
        }
        else if(type == InteractorType.Phone) {
            active = false;
            objectSwap.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.Lamp) {
            active = false;
            ColorGrading colorGrading;
            volume.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = lightsOff;
        }

        if(!active) {
            GetComponent<Outline>().enabled = false;
        }

        AudioSource newSFX = Instantiate(audioSource);
        newSFX.clip = clip;
        newSFX.Play();
    }

    public bool IsActive() {
        if(type == InteractorType.KotatsuPlugged || type == InteractorType.KotatsuUnplugged) {
            return false;
        }
        else return active;
    }

    public void Reset() {
        if(type == InteractorType.KotatsuPlugged) {
            active = true;
        }
        else if(type == InteractorType.KotatsuUnplugged) {
            active = false;
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.Lamp) {
            active = true;
            ColorGrading colorGrading;
            volume.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = lightsOn;
        }
        else if(type == InteractorType.Phone) {
            active = true;
            objectSwap.SetActive(false);
        }
        else if(type == InteractorType.TV) {
            active = true;
            GetComponent<MeshRenderer>().material = originalTexture;
        }
        else if(type == InteractorType.Door) {
            active = true;
            GetComponent<Animator>().Play("Original");
        }
    }
}
