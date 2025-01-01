using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using VisualGraphNodeSystem;
/// <summary>
/// 处理node 逻辑
/// </summary>
public class NodeProcesserTest : MonoBehaviour
{
    public Text text;
    public AudioSource audioSource;
    public NodeGraphBase nodeGraph;
    public Button[] btnOps;
    private NodeProcesser nodeProcesser;
    private void Start()
    {
        Debug.Log("开始");
        nodeProcesser = new NodeProcesser(nodeGraph);
        StartCoroutine(ProcessNode(nodeProcesser.GetFirstNode()));
    }
    private IEnumerator ProcessNode(VisualNodeBase currentNode)
    {
        if (currentNode == null)
        {
            Debug.Log("none");
            yield break;
        }

        nodeProcesser.RefreshNodeChange(currentNode);
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
    private VisualNodeBase LoadNextNode(VisualNodeBase currentNode, int outputPortIndex)
    {
        var port = currentNode.GetOutpotPortWithIndex(outputPortIndex);
        if (port != null)
            currentNode = port.GetConnectNode() as VisualNodeBase;
        else currentNode = null;
        return currentNode;
    }
}
