using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CoreLibrary
{
    public static class DynamicControls
    {
        public static void GenerateControl(HtmlGenericControl outerDivRow, DataRow topRow)
        {
            GenerateLabel(outerDivRow, topRow);

            switch (topRow["InputType"].ToString())
            {
                case "TextBox":
                    GenerateTextBox(outerDivRow, topRow);
                    break;

                case "TextArea":
                    GenerateTextArea(outerDivRow, topRow);
                    break;

                case "DatePicker":
                    GenerateDatePicker(outerDivRow, topRow);
                    break;

                case "CheckBox":
                    GenerateCheckBox(outerDivRow, topRow);
                    break;

                case "RadioButtonList":
                    GenerateRadioButtonList(outerDivRow, topRow);
                    break;

                case "DropDownList":
                    GenerateDropDownList(outerDivRow, topRow);
                    break;
                default:
                    break;
            }
        }

        private static void GenerateLabel(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl innerCaption = new HtmlGenericControl("div");
            innerCaption.Attributes["class"] = "col-1";
            innerCaption.InnerText = data["Caption"].ToString();

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                HtmlGenericControl spanElement = new HtmlGenericControl("span");
                spanElement.Attributes["style"] = "color: red";
                spanElement.InnerText = " *";
                innerCaption.Controls.Add(spanElement);
            }

            outerDivRow.Controls.Add(innerCaption);
        }

        private static void GenerateTextBox(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";

            String sFieldName = data["FieldName"].ToString();

            TextBox customTextBox = new TextBox();
            customTextBox.ID = "txt" + sFieldName;
            customTextBox.Text = String.Empty;
            customTextBox.ToolTip = data["Placeholder"].ToString();
            customTextBox.CssClass = "TextBoxStyle";
            customTextBox.Attributes["placeholder"] = data["Placeholder"].ToString();
            customTextBox.Enabled = false;

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customTextBox.Enabled = true;
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv"+ sFieldName;
                requiredFieldValidator.ControlToValidate = customTextBox.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
                requiredFieldValidator.EnableClientScript = true;
            }

            divControl.Controls.Add(customTextBox);
            outerDivRow.Controls.Add(divControl);
        }
        
        private static void GenerateTextArea(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";

            String sFieldName = data["FieldName"].ToString();

            TextBox customTextArea = new TextBox();
            customTextArea.ID = "txt" + sFieldName;
            customTextArea.TextMode = TextBoxMode.MultiLine;
            customTextArea.Rows = 3;
            customTextArea.Text = String.Empty;
            customTextArea.ToolTip = data["Placeholder"].ToString(); ;
            customTextArea.CssClass = "TextBoxStyle";
            customTextArea.Attributes["placeholder"] = data["Placeholder"].ToString(); ;
            customTextArea.Enabled = false;

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customTextArea.Enabled = true;
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv" + sFieldName;
                requiredFieldValidator.ControlToValidate = customTextArea.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.EnableClientScript = true;
            }

            divControl.Controls.Add(customTextArea);
            outerDivRow.Controls.Add(divControl);
        }

        private static void GenerateDatePicker(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";

            String sControlName = "txt" + data["FieldName"].ToString();

            TextBox customDatePicker = new TextBox();
            customDatePicker.ID = sControlName;
            customDatePicker.Text = String.Empty;
            customDatePicker.ToolTip = data["Placeholder"].ToString(); ;
            customDatePicker.CssClass = "TextBoxStyle";
            customDatePicker.Attributes["placeholder"] = data["Placeholder"].ToString(); ;
            customDatePicker.Enabled = false;

            CalendarExtender calExtender = new CalendarExtender();
            calExtender.ID = "calEx_" + sControlName;
            calExtender.PopupButtonID = customDatePicker.ID;
            calExtender.TargetControlID = customDatePicker.ID;
            calExtender.Format = "MM/dd/yyyy";
            calExtender.CssClass = "CalenderTheme";

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customDatePicker.Enabled = true;
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv" + data["FieldName"].ToString();
                requiredFieldValidator.ControlToValidate = customDatePicker.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.EnableClientScript = true;
            }

            divControl.Controls.Add(customDatePicker);
            divControl.Controls.Add(calExtender);
            outerDivRow.Controls.Add(divControl);
        }

        private static void GenerateCheckBox(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";


            String sCaption = data["Caption"].ToString();
            String sFieldName = data["FieldName"].ToString();
            String sPlaceholder = data["Placeholder"].ToString();
            String sFieldValue = data["Placeholder"].ToString();

            CheckBox customCheckBox = new CheckBox();
            customCheckBox.ID = "chk" + sFieldName;
            customCheckBox.ToolTip = sCaption;
            customCheckBox.CssClass = "CheckBoxStyle";
            customCheckBox.Enabled= false;

            if (sFieldValue == "1")
            {
                customCheckBox.Checked = true;
            }
            else
            {
                customCheckBox.Checked = false;
            }

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customCheckBox.Enabled = true;
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv" + data["FieldName"].ToString();
                requiredFieldValidator.ControlToValidate = customCheckBox.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.EnableClientScript = true;
            }

            divControl.Controls.Add(customCheckBox);
            outerDivRow.Controls.Add(divControl);
        }

        private static void GenerateRadioButtonList(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";


            String sCaption = data["Caption"].ToString();
            String sFieldName = data["FieldName"].ToString();
            String sPlaceholder = data["Placeholder"].ToString();

            RadioButtonList customRadioButton = new RadioButtonList();
            customRadioButton.ID = "rbtn" + sFieldName;
            customRadioButton.ToolTip = sCaption;
            customRadioButton.CssClass = "RadioButton";
            customRadioButton.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
            customRadioButton.Enabled= false;

            DataTable dtCategory = PopulateLists.GetCompanies();
            FillList.PopulateRadioButtonList(dtCategory, customRadioButton);

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customRadioButton.Enabled = true;
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv" + data["FieldName"].ToString();
                requiredFieldValidator.ControlToValidate = customRadioButton.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.EnableClientScript = true;
            }


            divControl.Controls.Add(customRadioButton);
            outerDivRow.Controls.Add(divControl);
        }

        private static void GenerateDropDownList(HtmlGenericControl outerDivRow, DataRow data)
        {
            HtmlGenericControl divControl = new HtmlGenericControl("div");
            divControl.Attributes["class"] = "col-2";

            String sCaption = data["Caption"].ToString();
            String sFieldName = data["FieldName"].ToString();
            String sPlaceholder = data["Placeholder"].ToString();
            String sFieldValue = data["Placeholder"].ToString();

            DropDownList customDropDownList = new DropDownList();
            customDropDownList.ID = "ddl" + sFieldName;
            customDropDownList.ToolTip = sCaption;
            customDropDownList.CssClass = "DropDownListStyle";
            customDropDownList.Enabled= false;

            DataTable dtCategory = PopulateLists.GetCompanies();
            FillList.PopulateDropDownList(dtCategory, customDropDownList, "Select Company");

            if (Convert.ToBoolean(data["IsEnable"].ToString()))
            {
                customDropDownList.Enabled = true;
            }

            if (sFieldValue != String.Empty)
            {
                customDropDownList.SelectedValue = sFieldValue;
            }
            else
            {
                customDropDownList.SelectedValue = "0";
            }

            if (Convert.ToBoolean(data["IsRequired"].ToString()))
            {
                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
                requiredFieldValidator.ID = "rfv" + data["FieldName"].ToString();
                requiredFieldValidator.ControlToValidate = customDropDownList.ID;
                requiredFieldValidator.ErrorMessage = data["RequiredMessage"].ToString();
                requiredFieldValidator.EnableClientScript = true;
            }

            divControl.Controls.Add(customDropDownList);
            outerDivRow.Controls.Add(divControl);
        }
    }
}
