using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Kutuphanem
{
    public class Animasyon : InputManager
    {
        private float maxSpeed;
        private float inputX;
        private float inputY;
        public void Deneme()
        {
            Debug.Log("Selam");
        }


        public void LeftMove(Animator anim, string animasyonName, string controlValue, float[] value)
        {
            if (left)
            {
                anim.SetBool(controlValue, true);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetFloat(animasyonName, value[0]);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(animasyonName, value[1]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(animasyonName, value[2]);
                }
                else
                {
                    anim.SetFloat(animasyonName, value[3]);

                }
            }

            if (!left)
            {
                anim.SetBool(controlValue, false);

                inputX = Mathf.Lerp(inputX,0,Time.deltaTime * 5);
                anim.SetFloat(animasyonName, inputX);
            }

        }

        public void RightMove(Animator anim, string animasyonName, string controlValue, float[] value)
        {
            if (right)
            {
                anim.SetBool(controlValue, true);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetFloat(animasyonName, value[0]);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(animasyonName, value[1]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(animasyonName, value[2]);
                }
                else
                {
                    anim.SetFloat(animasyonName, value[3]);

                }
            }

            if (!right)
            {
                anim.SetBool(controlValue, false);
                inputX = Mathf.Lerp(inputX,0,Time.deltaTime * 5);
                anim.SetFloat(animasyonName, inputX);
            }

        }

        public void BackwardMove(Animator anim,string animasyonName,string controlValue)
        {
            if (backward)
            {
                anim.SetBool(controlValue,true);
                anim.SetBool(animasyonName, true);
            }

            if (!backward)
            {
                anim.SetBool(controlValue,false);
                anim.SetBool(animasyonName, false);
            }
        }

        public void CrouchMove(Animator anim, string animasyonName,string controlValue, float[] value)
        {

            if (Input.GetKey(KeyCode.C))
            {
                anim.SetBool(controlValue,true);
                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(animasyonName, value[0]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(animasyonName, value[1]);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    anim.SetFloat(animasyonName, value[2]);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    anim.SetFloat(animasyonName, value[3]);
                }
                else
                {
                    anim.SetFloat(animasyonName, value[4]);

                }
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                anim.SetBool(controlValue,false);
                inputX = Mathf.Lerp(inputX,0,Time.deltaTime * 5);
                anim.SetFloat(animasyonName, inputX);
            }

        }


        public void CharacterMove(Animator anim,string speedValue,float maksimumUzunluk,float tamHiz, float yürümeHizi)
        {
            if (runing)
            {
                anim.SetBool("İleriAktifMi",true);
                maxSpeed = tamHiz;
            }
            else if (forward)
            {
                anim.SetBool("İleriAktifMi",true);
                maxSpeed = yürümeHizi;
                inputY = 1;
            }
            else
            {
                anim.SetBool("İleriAktifMi",false);
                maxSpeed = 0;
                inputY = Mathf.Lerp(inputY, 0, Time.deltaTime * 5);
            }
            anim.SetFloat(speedValue, Vector3.ClampMagnitude(new Vector3(0,0,inputY), maxSpeed).magnitude, maksimumUzunluk, Time.deltaTime * 10);
        }

        public void CharacterRotation(Camera mainCam,Transform transform)
        {
            Vector3 camOfset = mainCam.transform.forward;
            camOfset.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, camOfset, Time.deltaTime * 10f);
        }

        

    }



}
