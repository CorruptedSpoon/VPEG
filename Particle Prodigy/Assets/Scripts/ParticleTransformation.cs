﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTransformation : MonoBehaviour
{
    public ObjectManager objectManager;

    private bool isProton = false;
    private bool isNeutron = false;
    private bool isElectron = false;
    private bool isNucleus = false;
    private bool isAtom = false;

    private Particle particle;

    private void Start()
    {
        objectManager = GameObject.FindObjectOfType<ObjectManager>();

        if(gameObject.tag == "proton")
        {
            isProton = true;
        }
        else if(gameObject.tag == "neutron")
        {
            isNeutron = true;
        }
        else if (gameObject.tag == "electron")
        {
            isElectron = true;
        }
        else if(gameObject.tag == "nucleus")
        {
            isNucleus = true;
        }
        else if(gameObject.tag == "atom")
        {
            isAtom = true;
        }

        particle = gameObject.GetComponent<Force>().particle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Merge(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Merge(collider);
    }

    private void Merge(Collider2D collider)
    {
        string collType = collider.gameObject.tag;

        Force collForce = collider?.gameObject?.GetComponent<Force>();

        if (collForce == null)
        {
            return;
        }

        Particle collParticle = collForce.particle;

        //create a nucleus if a proton and neutron collide
        if (isProton && collType == "neutron")
        {
            //create new nucleus
            GameObject newNucleus = objectManager.InstantiateAtom(1, 1, 0, gameObject.transform.position);
            newNucleus.tag = "nucleus";

            ////remove proton and neutron from object mananger's lists
            //int indexToRemove = objectManager.protons.IndexOf(gameObject);
            //objectManager.protons.RemoveAt(indexToRemove);
            //indexToRemove = objectManager.neutrons.IndexOf(collider.gameObject);
            //objectManager.neutrons.RemoveAt(indexToRemove);

            ////destroy proton and neutron
            //Destroy(collider.gameObject);
            //Destroy(gameObject);

            objectManager.DestroyParticle(collider.gameObject);
            objectManager.DestroyParticle(gameObject);
        }
        //create an atom if a nucleus and an electron collide
        else if (isNucleus && collType == "electron")
        {
            //create new atom
            objectManager.InstantiateAtom(1, 1, 1, gameObject.transform.position);

            ////remove nucleus and electron from object manager's lists
            //int indexToRemove = objectManager.atoms.IndexOf(gameObject.GetComponent<Atom>());
            //objectManager.atoms.RemoveAt(indexToRemove);
            //indexToRemove = objectManager.electrons.IndexOf(collider.gameObject);
            //objectManager.electrons.RemoveAt(indexToRemove);

            ////destroy nucleus and electron
            //Destroy(gameObject);
            //Destroy(collider.gameObject);

            objectManager.DestroyParticle(gameObject);
            objectManager.DestroyParticle(collider.gameObject);
        }
        //combine atoms if they collide
        else if(particle == Particle.atom && collParticle == Particle.atom)
        {
            Atom atom1 = gameObject.GetComponent<Atom>();
            Atom atom2 = collider.gameObject.GetComponent<Atom>();

            if (objectManager.justAtomAtomCollided==0)
            {
                //create new atom
                objectManager.InstantiateAtom(
                    atom1.protons + atom2.protons,
                    atom1.neutrons + atom2.neutrons,
                    atom1.electrons + atom2.electrons,
                    gameObject.transform.position);

                //remove atoms from object manager
                int indexToRemove = objectManager.atoms.IndexOf(atom1);
                objectManager.atoms.RemoveAt(indexToRemove);
                indexToRemove = objectManager.atoms.IndexOf(atom2);
                objectManager.atoms.RemoveAt(indexToRemove);

                //destroy og atoms
                Destroy(atom1.gameObject);
                Destroy(atom2.gameObject);

                //hack to prevent crashing
                objectManager.justAtomAtomCollided = 3;
            }
            else
            {
                objectManager.justAtomAtomCollided--;
            }
        }
        //atom collides with electron change charge of atom
        else if(isAtom && collType == "electron")
        {
            //change atom's values
            Atom atom = gameObject.GetComponent<Atom>();
            atom.charge--;
            atom.electrons++;

            ////remove electron from object manager's list
            //int indexToRemove = objectManager.electrons.IndexOf(collider.gameObject);
            //objectManager.electrons.RemoveAt(indexToRemove);

            //destroy electron
            //Destroy(collider.gameObject);
            objectManager.DestroyParticle(collider.gameObject);
        }
        //atom collides with neutron change neutron count
        else if (isAtom && collType == "neutron")
        {
            //change atom's values
            Atom atom = gameObject.GetComponent<Atom>();
            atom.neutrons++;

            ////remove neutron from object manager's list
            //int indexToRemove = objectManager.neutrons.IndexOf(collider.gameObject);
            //objectManager.neutrons.RemoveAt(indexToRemove);

            //destroy electron
            //Destroy(collider.gameObject);
            objectManager.DestroyParticle(collider.gameObject);
        }
        //atom collides with proton change charge of atom
        else if (isAtom && collType == "proton")
        {
            //change atom's values
            Atom atom = gameObject.GetComponent<Atom>();
            atom.protons++;
            atom.charge++;

            ////remove neutron from object manager's list
            //int indexToRemove = objectManager.protons.IndexOf(collider.gameObject);
            //objectManager.protons.RemoveAt(indexToRemove);

            //destroy electron
            //Destroy(collider.gameObject);
            objectManager.DestroyParticle(collider.gameObject);
        }
    }
}
