using UnityEngine;
using TMPro;

public class DropDownMenus : MonoBehaviour
{
    private TMP_Dropdown tmpDropdown;
    private TextMeshProUGUI label;
    private SimFileHandler simFileHandler;

    void Start()
    {
        GameObject simFileHandlerObject = GameObject.Find("StateManager");
        simFileHandler = simFileHandlerObject.GetComponent<SimFileHandler>();

        Transform dropdownTransform = transform.Find("Canvas/Dropdown");

        if (dropdownTransform != null)
        {
            tmpDropdown = dropdownTransform.GetComponent<TMP_Dropdown>();

            if (tmpDropdown != null)
            {
                // Add a listener to the onValueChanged event
                tmpDropdown.onValueChanged.AddListener(OnOptionSelected);

                // Get a reference to the Label component
                Transform labelTransform = tmpDropdown.transform.Find("Label");
                label = labelTransform.GetComponent<TextMeshProUGUI>();

                // Change the label text
                label.text = "Load";
            }
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        // Check if the first option was selected
        if (optionIndex == 0)
        {
            simFileHandler.OpenTextFileLoadDialog();
        }
        if (optionIndex == 1)
        {
            simFileHandler.OpenSimStateLoadDialog();
        }
    }
}
