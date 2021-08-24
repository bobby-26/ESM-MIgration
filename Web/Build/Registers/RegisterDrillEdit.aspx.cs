using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;

public partial class Registers_RegisterDrillEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripdrilladdmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!Page.IsPostBack)
        {
            
            DataTable dt1 = PhoenixRegisterDrill.vessellist();

            radcbappliesto.DataSource = dt1;
            radcbappliesto.DataBind();

            DataTable dt2 = PhoenixRegisterDrill.ListOfVessels();
            radexcludedvessels.DataSource = dt2;
            radexcludedvessels.DataBind();

            ViewState["DRILLID"] = General.GetNullableGuid(Request.QueryString["drillid"]);
            Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());

            DataTable dt = PhoenixRegisterDrill.drilleditlist(drillid);
            if (dt.Rows.Count > 0)
            {
                Raddrillnameentry.Text = dt.Rows[0]["FLDDRILLNAME"].ToString();
                tbfrequencyentry.Text = dt.Rows[0]["FLDFREQUENCY"].ToString();

                RadRadioButtonfrequencytype.SelectedValue = dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();

                General.RadBindCheckBoxList(radcbappliesto, dt.Rows[0]["FLDAPPLIESTO"].ToString());
                RadRadioButtontype.SelectedValue = dt.Rows[0]["FLDTYPE"].ToString();
                RadRadioButtonfixedorvariable.SelectedValue = dt.Rows[0]["FLDFIXEDORVARIABLE"].ToString();
                General.RadBindComboBoxCheckList(radexcludedvessels, dt.Rows[0]["FLDEXCLUDEDVESSELS"].ToString());
                if (dt.Rows[0]["FLDAFFECTEDBYCREWCHANGEYN"].ToString() == "Yes")
                {
                    RadRadioaffectbycrewynlist.SelectedValue = "Yes";
                    crewchangelabel.Visible = true;
                    crewchangecolon.Visible = true;
                    tbcrewpercententry.Visible = true;
                }
                else
                {
                    RadRadioaffectbycrewynlist.SelectedValue = "No";

                }
                if (dt.Rows[0]["FLDCREWCHANGEPERCENTAGE"] != null)
                {
                    tbcrewpercententry.Text = dt.Rows[0]["FLDCREWCHANGEPERCENTAGE"].ToString();
                    dummycrewpercentage.Text = dt.Rows[0]["FLDCREWCHANGEPERCENTAGE"].ToString();
                }

                RadRadiophotoynlist.SelectedValue = dt.Rows[0]["FLDPHOTOYN"].ToString();


                RadRadioButtondashboardynlist.SelectedValue = dt.Rows[0]["FLDDASHBOARDYN"].ToString();

            }

        }


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
    protected void drilladdmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                string drillname = General.GetNullableString(Raddrillnameentry.Text);
                int? frequency = General.GetNullableInteger(tbfrequencyentry.Text);
                string frequencytype = RadRadioButtonfrequencytype.SelectedValue;
                string appliesto = General.GetNullableString(General.RadCheckBoxList(radcbappliesto));
                string fixedorvariable = RadRadioButtonfixedorvariable.SelectedValue;
                string type = RadRadioButtontype.SelectedValue;
                string affectedbycrewchange = RadRadioaffectbycrewynlist.SelectedValue;
                int? crewchangepercentage = General.GetNullableInteger(tbcrewpercententry.Text);
                string photoyn = RadRadiophotoynlist.SelectedValue;
                string showondashboard = RadRadioButtondashboardynlist.SelectedValue;
                if (!IsValidDrillDetails(drillname, frequency, appliesto, affectedbycrewchange, crewchangepercentage, frequencytype))
                {
                    ucError.Visible = true;
                    return;
                }
                string newapplicabletype = string.Empty;
                Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string excludedvessels = string.Empty;
                excludedvessels = General.GetNullableString(GetCheckedItemsvalues(radexcludedvessels, excludedvessels));

                PhoenixRegisterDrill.drillupdate(rowusercode
                                                , drillname
                                                , frequency
                                                , frequencytype
                                                , appliesto
                                                , photoyn
                                                , showondashboard
                                                , drillid
                                                , fixedorvariable
                                                , type
                                                , affectedbycrewchange
                                                , crewchangepercentage
                                                , excludedvessels
                                               );
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
    private bool IsValidDrillDetails(string drill, int? frequency, string appliesto, string crewchangeyn, int? crewpercentage, string frequencytype)
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
}