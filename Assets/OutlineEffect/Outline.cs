/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace cakeslice
{
    [RequireComponent(typeof(Renderer))]
    public class Outline : MonoBehaviour
    {
        public QuikOutline quikOutline;

        public Renderer Renderer { get; private set; }
        public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
        public MeshFilter MeshFilter { get; private set; }

        public int color;
        public bool eraseRenderer;

        public bool Hover3sec;
        public float counter;

        public bool isbordervisible = false;

        //float xmin;
        //float ymin;
        //float zmin;
        //float xmax;
        //float ymax;
        //float zmax = -500;

        private void Awake()
        {
            quikOutline = GetComponent<QuikOutline>();
            Renderer = GetComponent<Renderer>();
            SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            MeshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            if (Hover3sec && isbordervisible)
            {

                counter += Time.deltaTime;
                if (counter > 0.05f)
                {
                    Hover3sec = false;
                    //if (CanvasManager.instance.snapping == CamSnap.Center)
                    {
                    }
                }
            }
        }


        private Material[] _SharedMaterials;
        public Material[] SharedMaterials
        {
            get
            {
                if (_SharedMaterials == null)
                    _SharedMaterials = Renderer.sharedMaterials;

                return _SharedMaterials;
            }
        }

        private void OnMouseEnter()
        {

        }

        public void SetSingleDimensions()
        {
            if (transform.parent.gameObject.name == "Drawer10Base40")
            {

            }

            else if (transform.parent.gameObject.name == "Pantry")
            {

            }

            else if (transform.parent.gameObject.name == "Freezer" || transform.parent.gameObject.name == "Fridge")
            {

            }

            else if (transform.parent.gameObject.name == "CornerCabinet")
            {

            }

            else if (transform.parent.gameObject.name == "Drawer5Base40" ||
                transform.parent.gameObject.name == "Drawer3Base40" ||
                transform.parent.gameObject.name == "Door2Base" ||
                transform.parent.gameObject.name == "SinkBase")
            {

            }

            else if (transform.parent.gameObject.name == "Drawer6Base20" ||
                transform.parent.gameObject.name == "Drawer3Base20" ||
                transform.parent.gameObject.name == "LeftDoor1Base" ||
                transform.parent.gameObject.name == "RightDoor1Base" ||
                transform.parent.gameObject.name == "BeverageCenter")
            {

            }

            else if (transform.parent.gameObject.name == "Door2Base62")
            {

            }

            else if (transform.parent.gameObject.name == "TallCabinet")
            {

            }

            else if (transform.parent.gameObject.name == "LeftLocker" ||
                transform.parent.gameObject.name == "RightLocker")
            {

            }

            else if (transform.parent.gameObject.name.Contains("WallMount"))
            {
                if (transform.parent.gameObject.name.Contains("40"))
                {
                }
                if (transform.parent.gameObject.name.Contains("20"))
                {

                }

            }

            else if (transform.parent.gameObject.name.Contains("OverHeads"))
            {
                if (transform.parent.gameObject.name.Contains("40"))
                {

                }
                if (transform.parent.gameObject.name.Contains("60"))
                {

                }
                if (transform.parent.gameObject.name.Contains("80"))
                {

                }

            }

            else if (transform.parent.gameObject.name.Contains("Stainless") || transform.parent.gameObject.name.Contains("PowderCoated"))
            {
                if (transform.parent.gameObject.name.Contains("20"))
                {

                }
                if (transform.parent.gameObject.name.Contains("40"))
                {
                }
                if (transform.parent.gameObject.name.Contains("60"))
                {

                }
                if (transform.parent.gameObject.name.Contains("80"))
                {

                }
                if (transform.parent.gameObject.name.Contains("62"))
                {

                }

            }

            else if (transform.parent.gameObject.name.Contains("Aluminium") || transform.parent.gameObject.name.Contains("Procore"))
            {
                if (transform.parent.gameObject.name.Contains("40"))
                {

                }
                if (transform.parent.gameObject.name.Contains("60"))
                {

                }
                if (transform.parent.gameObject.name.Contains("80"))
                {

                }
                if (transform.parent.gameObject.name.Contains("62"))
                {

                }
            }

            if (transform.parent.gameObject.name == "TallCabinet" || transform.parent.gameObject.name == "LeftLocker" || transform.parent.gameObject.name == "RightLocker")
            {

            }
            else
            {

            }

        }


        private void OnMouseExit()
        {
            Hover3sec = false;




            //CanvasManager.instance.singleWidth = 0;
            //CanvasManager.instance.singleHeight = 0;
            //CanvasManager.instance.singleBreadth = 0;

            //CanvasManager.instance.diamensionObject.SetActive(false);
        }

        public void OnMouseUp()
        {

        }


        public void Up()
        {
            StartCoroutine(showborder());
        }

        IEnumerator showborder()
        {
            yield return new WaitForSeconds(0.1f);
        }


        public void Enable()
        {
            quikOutline.enabled = true;
            //OutlineEffect.Instance?.AddOutline(this);
            isbordervisible = true;
        }

        public void Disable()
        {
            quikOutline.enabled = false;
            //CanvasManager.instance.GeneratedModel.transform.Find("Bin").gameObject.SetActive(false);
            //OutlineEffect.Instance?.RemoveOutline(this);
            isbordervisible = false;
        }

    }
}
