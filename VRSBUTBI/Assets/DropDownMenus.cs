using UnityEngine;
using TMPro;

public class DropDownMenus : MonoBehaviour
{
    private TMP_Dropdown tmpDropdown;
    private TextMeshProUGUI label;
    private SimFileHandler simFileHandler;

    private bool firstOptionSelected = true;

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

                // Set the selected index to -1 to avoid the default selection
                tmpDropdown.value = -1;
            }
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        // Ignore the first call to this method if the first option is selected as a default value
        if (firstOptionSelected)
        {
            firstOptionSelected = false;
            return;
        }

        // Handle the selected option
        if (optionIndex == 0)
        {
            simFileHandler.OpenTextFileLoadDialog();
        }
        else if (optionIndex == 1)
        {
            simFileHandler.OpenSimStateLoadDialog();
        }
    }
}
