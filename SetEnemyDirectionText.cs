using TMPro;
using UnityEngine;

public class SetEnemyDirectionText : MonoBehaviour
{
    [SerializeField]
    private string prefix;
    
    [SerializeField]
    private string direction;

    [SerializeField]
    private TMP_Text text;
        
    private void Update()
    {
        text.text = string.Concat(prefix, direction);
    }
}
