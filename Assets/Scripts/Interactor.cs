using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Interactor : MonoBehaviour
{

    public int value = 1;
    public enum InteractorType {Collectible, TV, KotatsuPlug, Door, Lamp, Phone, Inactive};
    public InteractorType type = InteractorType.Collectible;
    public Material textureSwap;
    public GameObject objectSwap;
    public string animation;
    public PostProcessVolume volume;
    public Color lightsOff;
    Color lightsOn = new Color(1,1,1,1);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if(type != InteractorType.Inactive && collider.tag == "InteractorField") {
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
            type = InteractorType.Inactive;
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.KotatsuPlug) {
            objectSwap.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.Door) {
            type = InteractorType.Inactive;
            GetComponent<Animator>().Play(animation);
        }
        else if(type == InteractorType.TV) {
            type = InteractorType.Inactive;
            GetComponent<MeshRenderer>().material = textureSwap;
        }
        else if(type == InteractorType.Phone) {
            type = InteractorType.Inactive;
            objectSwap.SetActive(true);
            gameObject.SetActive(false);
        }
        else if(type == InteractorType.Lamp) {
            ColorGrading colorGrading;
            volume.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = lightsOff;
        }

        if(type == InteractorType.Inactive) {            
            GetComponent<Outline>().enabled = false;
        }
    }

    public bool IsActive() {
        return type == InteractorType.Inactive;
    }

    public void Reset() {
        if(type == InteractorType.Lamp) {
            ColorGrading colorGrading;
            volume.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = lightsOn;
        }
    }
}
