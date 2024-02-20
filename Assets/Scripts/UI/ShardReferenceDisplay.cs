using TMPro;
using UnityEngine;

public class ShardReferenceDisplay : MonoBehaviour
{
    [SerializeField] ShardNumReference shardNum;
    private TextMeshProUGUI text;
    private void Awake() { text = GetComponent<TextMeshProUGUI>(); }

    private void OnEnable() { shardNum.OnChange += UpdateText; }

    private void OnDisable() { shardNum.OnChange -= UpdateText; }

    private void UpdateText() { text.text = shardNum.Value + " SHARDS COLLECTED"; }
}
