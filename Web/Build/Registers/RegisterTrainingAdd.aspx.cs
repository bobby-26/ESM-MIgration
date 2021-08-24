using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class Registers_RegisterTrainingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        TabstripTrainingaddmenu.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        {
           

            DataTable dt = PhoenixRegisterTraining.vessellist();

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

    protected void Trainingaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string Trainingname = General.GetNullableString(RadTrainingnameentry.Text);
                int? frequency = General.GetNullableInteger(tbfrequencyentry.Text);
                string frequencytype = RadRadioButtonfrequencytype.SelectedValue;
                string appliesto = General.GetNullableString(GetCheckedItemsvalues(radcbappliesto));
                string fixedorvariable = RadRadioButtonfixedorvariable.SelectedValue;
                string type = RadRadioButtontype.SelectedValue;
               
                string photoyn = RadRadiophotoynlist.SelectedValue;
                string showondashboard = RadRadioButtondashboardynlist.SelectedValue;
                if (!IsValidTrainingDetails(Trainingname, frequency, appliesto, frequencytype))
                {
                    ucError.Visible = true;
                    return;
                }
                string excludedvessels = string.Empty;
                excludedvessels = General.GetNullableString(GetCheckedItemsvalues(radexcludedvessels, excludedvessels));
                Guid? TrainingId = Guid.Empty;
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                PhoenixRegisterTraining.TrainingInsert(rowusercode
                                                , Trainingname
                                                , frequency
                                                , frequencytype
                                                , appliesto
                                                , photoyn
                                                , showondashboard
                                                , fixedorvariable
                                                , type
                                                , ref TrainingId
                                                , excludedvessels);


                DateTime? lastdonedate = null;

                PhoenixRegisterTraining.TrainingScheduleInsert(rowusercode, TrainingId, lastdonedate);

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

    private bool IsValidTrainingDetails(string Training, int? frequency, string appliesto,  string frequencytype)
    {

        ucError.HeaderMessage = "Provide the following required information";


        if (Training == null)
        {
            ucError.ErrorMessage = "Training name.";
        }
        if (frequency == null)
        {
            ucError.ErrorMessage = "Frequency.";
        }
        if (appliesto == null)
        {
            ucError.ErrorMessage = "Vessel types to which the drill is applicable.";
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
}