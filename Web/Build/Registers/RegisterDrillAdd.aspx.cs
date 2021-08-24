using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class Registers_RegisterDrillAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripdrilladdmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!Page.IsPostBack)
        {
           

            DataTable dt = PhoenixRegisterDrill.vessellist();
            radcbappliesto.DataSource = dt;
            radcbappliesto.DataBind();
            

            DataTable dt2 = PhoenixRegisterDrill.ListOfVessels();
            radexcludedvessels.DataSource = dt2;
            radexcludedvessels.DataBind();

            foreach (ButtonListItem item in radcbappliesto.Items)
            {
                 item.Selected = true;
            }

            RadRadioButtonfrequencytype.SelectedValue = "Months";
            RadRadioButtonfixedorvariable.SelectedValue = "Fixed";
            RadRadioButtontype.SelectedValue = "Mandatory";
            RadRadioaffectbycrewynlist.SelectedValue = "No";
            RadRadiophotoynlist.SelectedValue = "Yes";
            RadRadioButtondashboardynlist.SelectedValue = "Yes";
        }
    }

    protected static string GetCheckedItemsvalues(RadCheckBoxList checkbox)
    {

        var sb = new StringBuilder();
        var collection = checkbox.SelectedValues;
        string checkednames = string.Empty;
        if (collection.Length > 0)
        {

            foreach (var item in collection)
                sb.Append(item + ",");


            checkednames = sb.ToString();
        }

        return checkednames;


    }
    protected void drilladdmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string drillname = General.GetNullableString(Raddrillnameentry.Text);
                int? frequency = General.GetNullableInteger(tbfrequencyentry.Text);
                string frequencytype = RadRadioButtonfrequencytype.SelectedValue;
                string appliesto = General.GetNullableString(GetCheckedItemsvalues(radcbappliesto));
                string fixedorvariable = RadRadioButtonfixedorvariable.SelectedValue;
                string type = RadRadioButtontype.SelectedValue;
                string affectedbycrewchange = RadRadioaffectbycrewynlist.SelectedValue;
                int? crewchangepercentage = General.GetNullableInteger(tbcrewpercententry.Text);
                string photoyn = RadRadiophotoynlist.SelectedValue;
                string showondashboard = RadRadioButtondashboardynlist.SelectedValue;
                string excludedvessels = string.Empty;
                excludedvessels = General.GetNullableString(GetCheckedItemsvalues(radexcludedvessels, excludedvessels));
                if (!IsValidDrillDetails(drillname, frequency, appliesto, affectedbycrewchange, crewchangepercentage , frequencytype))
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? drillid = Guid.Empty;
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixRegisterDrill.drillinsert( rowusercode
                                                , drillname
                                                , frequency
                                                , frequencytype
                                                , appliesto
                                                , photoyn
                                                , showondashboard
                                                , fixedorvariable
                                                , type
                                                , affectedbycrewchange
                                                , crewchangepercentage
                                                , ref drillid
                                                , excludedvessels);
                

                DateTime? lastdonedate = null;

                PhoenixRegisterDrill.drillscheduleinsert(rowusercode, drillid, lastdonedate);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    private bool IsValidDrillDetails(string drill, int? frequency,  string appliesto, string crewchangeyn, int? crewpercentage , string frequencytype)
    {

        ucError.HeaderMessage = "Provide the following required information";


        if (drill == null)
        {
            ucError.ErrorMessage = "Drill name.";
        }
        if (frequency == null)
        {
            ucError.ErrorMessage = "Frequency.";
        }
        if (appliesto == null)
        {
            ucError.ErrorMessage = "Vessel types to which the drill is applicable.";
        }
        if (crewchangeyn == "Yes" && crewpercentage == null)
        {
            ucError.ErrorMessage = "Crew Change Percentage";
        }
        if (frequencytype == "Months" && frequency > 12)
        {
            ucError.ErrorMessage = "Valid Frequency (1-12)";
        }

        if (frequencytype == "Weeks" && frequency > 52)
        {
            ucError.ErrorMessage = "Valid Frequency (1-52)";
        }

        return (!ucError.IsError);
    }

    protected static string GetCheckedItemsvalues(RadComboBox comboBox, string checkednames)
    {
        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {
            foreach (var item in collection)
                sb.Append(item.Value + ",");
            checkednames = sb.ToString();
        }
        return checkednames;
    }
    protected void crewchange(object sender, EventArgs e)
    {
        if (RadRadioaffectbycrewynlist.SelectedValue == "Yes")
        {
            crewchangelabel.Visible = true;
            crewchangecolon.Visible = true;
            tbcrewpercententry.Visible = true;
        }
        if (RadRadioaffectbycrewynlist.SelectedValue == "No")
        {
            crewchangelabel.Visible = false;
            crewchangecolon.Visible = false;
            tbcrewpercententry.Visible = false;
            tbcrewpercententry.Text = "";
        }

    }
}