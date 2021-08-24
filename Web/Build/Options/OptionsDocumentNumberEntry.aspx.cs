using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;


public partial class OptionsDocumentNumberEntry : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                toolbar.AddButton("Cancel", "CANCEL");
                Menudocument.MenuList = toolbar.Show();
                hdnSeparator.Value = "";
                hdnColumns.Value = "";
                BindDocumentType();
                Session["mskval"] = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true; 
        }
    }
    public void BindDocumentType()
    {
        ddlDocumentType.DataSource = PhoenixDocumentNumber.GetDocumentTypeList(48);
        ddlDocumentType.DataBind();
    }

    protected void txtFormat_TextChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnRules_Click(object sender, EventArgs e)
    {        
        
         
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbColumnsList.SelectedIndex > -1)
            {

                string _value = lbColumnsList.SelectedItem.Value; //Gets the value of  items in list.
                string _text = lbColumnsList.SelectedItem.Text;  // Gets the Text of items in the list.  
                ListItem item = new ListItem(); //create a list item
                item.Text = _text;               //Assign the values to list item   
                item.Value = _value;
                int i = 0;
                if (!(item.Text == "-" || item.Text == "/" || item.Text == "."))
                {
                    foreach (ListItem list in lbColumnsAssign.Items)
                    {
                        if (list.Text == item.Text)
                        {
                            i++;
                        }
                        else if ((list.Text == "Year(2)" && item.Text == "Year(4)") || (list.Text == "Year(4)" && item.Text == "Year(2)"))
                        {
                            i++;
                            ucError.ErrorMessage = "You can select only one year format";
                            ucError.Visible = true;
                            return;

                        }
                        else if ((list.Text == "Total" && item.Text == "This Year") || (list.Text == "This Year" && item.Text == "Total"))
                        {
                            i++;
                            ucError.ErrorMessage = "You can select only one serial numer (This Year or Total)";
                            ucError.Visible = true;
                            return;
                        }

                    }
                }
                if (i == 0)
                {
                    if (item.Text == "Total")
                    {
                        if (txtserial.Text.Length > 0)
                            PhoenixDocumentNumber.TotalNumber = txtserial.Text;
                        else
                            PhoenixDocumentNumber.TotalNumber = "000001";
                    }
                    if (lbColumnsAssign.Items.Count > 0)
                    {
                        hdnSeparator.Value = hdnSeparator.Value + item.Text;
                        hdnColumns.Value = hdnColumns.Value + item.Value;
                    }
                    else
                    {
                        hdnSeparator.Value = _text;
                        hdnColumns.Value = _value;
                    }
                    lbColumnsAssign.Items.Add(item);
                    lbColumnsList.Items.Remove(item);
                    btnTest_Click(null, null);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbColumnsAssign.SelectedIndex > -1)
            {
                string _value = lbColumnsAssign.SelectedItem.Value; //Gets the value of items in list.
                string _text = lbColumnsAssign.SelectedItem.Text;  // Gets the Text of items in the list.  
                ListItem item = new ListItem(); //create a list item
                item.Text = _text;               //Assign the values to list item   
                item.Value = _value;
                int foundS1 = hdnSeparator.Value.IndexOf(_text);
                int foundS2 = hdnColumns.Value.IndexOf(_value);

                if (foundS1 != 0)
                {

                    hdnSeparator.Value = hdnSeparator.Value.Remove(foundS1, _text.Length);
                    hdnColumns.Value = hdnColumns.Value.Remove(foundS2, _value.Length);
                }
                else
                {
                    if (lbColumnsAssign.Items.Count > 1)
                    {
                        hdnSeparator.Value = hdnSeparator.Value.Remove(foundS1, _text.Length);
                        hdnColumns.Value = hdnColumns.Value.Remove(foundS2, _value.Length);
                    }
                    else
                    {
                        hdnSeparator.Value = hdnSeparator.Value.Remove(foundS1, _text.Length);
                        hdnColumns.Value = hdnColumns.Value.Remove(foundS2, _value.Length);
                    }
                }
                lbColumnsAssign.Items.Remove(item); //Remove from the selected list
                if (item.Value.Substring(0, 1) == "F")
                {
                    lbColumnsList.Items.Add(item);
                }
                btnTest_Click(null, null);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        try
        {

            foreach (ListItem item in lbColumnsAssign.Items)
            {
                if (item.Value.Substring(0, 1) == "F")
                {
                    lbColumnsList.Items.Add(item);
                }
            }
            lbColumnsAssign.Items.Clear();
            hdnSeparator.Value = "";
            hdnColumns.Value = "";
            btnTest_Click(null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        try
        {
            string docNumber = "";
            if (hdnSeparator.Value.Length > 0)
            {

                foreach (ListItem litem in lbColumnsAssign.Items)
                {
                    docNumber = docNumber + countFunction(litem.Text);
                }
            }
            txtRules.Text = docNumber.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public string countFunction(string val)
    {
        switch(val) 
        {
            case "Year(2)":
                return DateTime.Now.ToString("yy");               
            case "Year(4)":
                return DateTime.Now.ToString("yyyy");               
            case "Instalation Number":
                return DateTime.Now.ToString("yy");              
            case "Department Number":
                return "ACC";               
            case "Vessel Short Code":
                return "NOB";                
            case "Document Short Code":
                return "RFQ";                
            case "Company Short Code":
                return "ESM";                
            case "Currency":
                return "USD";                
            case "Total":
                return PhoenixDocumentNumber.TotalNumber;               
            case "This Year":
                return "000001";               
            case "This Month":
                return "000001";
               
            case "This Week":
                return "01";
               
            case "Current User":
                return "00001";
                
        }
        return val;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int countCheck = 0;
        if(ddlDocumentType.SelectedIndex <=0)
        {
            ucError.ErrorMessage = "Document type is required. Please select";
            ucError.Visible = true;            
            return;
        }
        string columns = "";
        foreach(ListItem litem in lbColumnsAssign.Items)
        {
            if(litem.Text =="Company Short Code")
            {
                PhoenixDocumentNumber.CompanyCode="Yes";
                columns = columns + "@COMPANYCODE + ";
            }
            else if (litem.Text == "Currency")
            {
                PhoenixDocumentNumber.CurrencyCode = "Yes";
                columns = columns + " @CURRENCY +";
            }
            else if (litem.Text == "Document Short Code")
            {
                PhoenixDocumentNumber.DocumentCode = "Yes";
                columns = columns + " @DOCUMENTCODE + ";
            }
            else if (litem.Text == "Vessel Short Code")
            {
                PhoenixDocumentNumber.VesselCode = "Yes";              
                columns = columns + " @VESSELCODE +";
            }
            else if (litem.Text == "Total")
            {
                columns = columns + " @CURRETNUMBER ";
                countCheck++;
            }
            else if (litem.Text == "Year(2)")
            {
                PhoenixDocumentNumber.YearFormat = "yy";
                columns = columns + " RIGHT(YEAR(GETDATE()),2) +";
            }
            else if (litem.Text == "Year(4)")
            {
                PhoenixDocumentNumber.YearFormat = "yyyyy";
                columns = columns + " YEAR(GETDATE()) +";
            }
            else
            {
                columns = columns + "'"+ litem.Text + "' + ";
            }
            
        }
        if (countCheck > 0)
        {

            PhoenixDocumentNumber.SaveDocumentNumber(Convert.ToInt32(ddlDocumentType.SelectedValue.ToString()), "Vessel", PhoenixDocumentNumber.DocumentCode, PhoenixDocumentNumber.VesselCode, PhoenixDocumentNumber.CompanyCode
                , PhoenixDocumentNumber.CurrencyCode, PhoenixDocumentNumber.YearFormat, PhoenixDocumentNumber.TotalNumber, columns, "yes");
        }
        else
        {
            ucError.ErrorMessage = "Please select Total for serial number";
            ucError.Visible = true;     
            return;
        }
        btnClearAll_Click(null, null);  
    }
   
    protected void btnCustom_Click(object sender, EventArgs e)
    {
        if (txtCustom.Text.Length < 1)
        {
            ucError.ErrorMessage = "'Please put custom values in custemtext box";
            ucError.Visible = true;
            return;
        }
        else 
        {
        ListItem item = new ListItem(); //create a list item
        item.Text = txtCustom.Text;               //Assign the values to list item   
        item.Value = txtCustom.Text + lbColumnsAssign.Items.Count;
        lbColumnsAssign.Items.Add(item);
        hdnSeparator.Value = hdnSeparator.Value + item.Text;
        hdnColumns.Value = hdnColumns.Value + item.Value;
           
        }
        btnTest_Click(null, null);  
    }
    protected void Menudocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                btnSave_Click(null, null);
            }
            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

