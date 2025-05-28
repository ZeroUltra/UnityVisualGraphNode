using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VisualGraphNodeSystem;
using System.Threading.Tasks;
using System;
namespace VisualGraphNodeSystem.Test
{
    /// <summary>
    /// 处理node 逻辑
    /// </summary>
    public class NodeProcesserTest : MonoBehaviour
    {
        public Text text;
        public AudioSource audioSource;
        public VisualGraph nodeGraph;
        public Button[] btnOps;
       
        private void Start()
        {
            Debug.Log("开始");
            MyNodeProcesser myNodeProcesser = new MyNodeProcesser(nodeGraph);
            myNodeProcesser.text = text;
            myNodeProcesser.audioSource = audioSource;
            myNodeProcesser.nodeGraph = nodeGraph;
            myNodeProcesser.btnOps = btnOps;
            myNodeProcesser.Process(myNodeProcesser.GetFirstNode());
        }
    }

    public class MyNodeProcesser : NodeProcesser
    {
        public Text text;
        public AudioSource audioSource;
        public VisualGraph nodeGraph;
        public Button[] btnOps;

        public MyNodeProcesser(VisualGraph nodeGraph) : base(nodeGraph)
        {

        }
        public override async void Process(VisualNodeBase currentNode)
        {
            base.Process(currentNode);
            if (currentNode != null)
            {
                if (currentNode is NodeWait nodeWait)
                {
                    await Task.Delay(TimeSpan.FromSeconds(nodeWait.waitDuration));
                    Process(GetNextNodeWithOutputPort(currentNode, 0));
                }
                else if (currentNode is NodeText nodeText)
                {
                    text.text = nodeText.Text;
                    Process(GetNextNodeWithOutputPort(currentNode, 0));
                }
                else if (currentNode is NodePlayAudio nodePlayAudio)
                {
                    audioSource.clip = nodePlayAudio.AudioClip;
                    audioSource.Play();
                    Process(GetNextNodeWithOutputPort(currentNode, 0));

                }
                else if (currentNode is NodeStopAudio nodeStopAudio)
                {
                    audioSource.Stop();
                    Process(GetNextNodeWithOutputPort(currentNode, 0));
                }

                else if (currentNode is NodeOption nodeMenu)
                {
                    btnOps[0].transform.parent.gameObject.SetActive(true);
                    for (int i = 0; i < btnOps.Length; i++)
                    {
                        int index = i;
                        btnOps[i].onClick.RemoveAllListeners();
                        btnOps[i].GetComponentInChildren<Text>().text = nodeMenu[index];
                        btnOps[i].onClick.AddListener(() =>
                        {
                            btnOps[0].transform.parent.gameObject.SetActive(false);
                            Process(GetNextNodeWithOutputPort(currentNode, index));
                        });
                    }
                }
                else if (currentNode is NodeEnd nodeEnd)
                {
                    Debug.Log("结束");
                }
            }
        }
    }
}