using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using VisualGraphNodeSystem;
/// <summary>
/// 处理node 逻辑
/// </summary>
public class NodeProcesserTest : NodeProcesser
{
    public Text text;
    public AudioSource audioSource;

    public Button[] btnOps;

    private void Start()
    {
        Debug.Log("开始");
        var currentNode = InitProcesser();
        StartCoroutine(ProcessNode(currentNode));
    }
    private IEnumerator ProcessNode(NodeBase currentNode)
    {
        if (currentNode == null)
        {
            Debug.Log("none");
            yield break;
        }
       
        RefreshNodeChange(currentNode);
        Debug.Log(currentNode.name);
        if (currentNode is NodeWait nodeWait)
        {
            yield return new WaitForSeconds(nodeWait.waitDuration);
            StartCoroutine(ProcessNode(LoadNextNode(currentNode, 0)));
            yield break;
        }
        else if (currentNode is NodeText nodeText)
        {
            text.text = nodeText.Text;
            StartCoroutine(ProcessNode(LoadNextNode(currentNode, 0)));
            yield break;
        }
        else if (currentNode is NodePlayAudio nodePlayAudio)
        {
            audioSource.clip = nodePlayAudio.AudioClip;
            audioSource.Play();
            StartCoroutine(ProcessNode(LoadNextNode(currentNode, 0)));
            yield break;
        }
        else if (currentNode is NodeStopAudio nodeStopAudio)
        {
            audioSource.Stop();
            StartCoroutine(ProcessNode(LoadNextNode(currentNode, 0)));
            yield break;
        }

        else if (currentNode is NodeMenu nodeMenu)
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

                    StartCoroutine(ProcessNode(LoadNextNode(currentNode, index)));
                });
            }
            yield break;
        }
        else if (currentNode is NodeEnd nodeEnd)
        {
            Debug.Log("结束");
        }
    }

    /// <summary>
    /// 获取下一个node
    /// </summary>
    /// <param name="currentNode">当前node</param>
    /// <param name="outputPortIndex">port index</param>
    /// <returns></returns>
    private NodeBase LoadNextNode(NodeBase currentNode, int outputPortIndex)
    {
        var port = currentNode.GetOutpotPortWithIndex(outputPortIndex);
        if (port != null)
            currentNode = port.GetConnectNode() as NodeBase;
        else currentNode = null;
        return currentNode;
    }
}
