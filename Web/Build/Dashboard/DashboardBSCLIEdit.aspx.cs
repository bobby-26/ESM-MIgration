using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class Dashboard_DashboardBSCLIEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        {
            ViewState["LIID"] = General.GetNullableGuid(Request.QueryString["shippingliid"]);
            DataTable dt = PhoenixDashboardBSCLI.LIUnitList();

            RadCbliunit.DataSource = dt;
            RadCbliunit.DataTextField = "FLDUNIT";
            RadCbliunit.DataValueField = "FLDUNITID";
            RadCbliunit.DataBind();


            DataTable dt1 = PheonixDashboardSKKPI.Departmentlist();

            radcbdept.DataSource = dt1;
            radcbdept.DataTextField = "FLDDEPARTMENTNAME";
            radcbdept.DataValueField = "FLDDEPARTMENTID";
            radcbdept.DataBind();



            DataTable dt2 = PhoenixDashboardBSCLI.LIEditList(General.GetNullableGuid(ViewState["LIID"].ToString()));
            if (dt2.Rows.Count > 0)
            {
                Radliidentry.Text = dt2.Rows[0]["FLDLICODE"].ToString();
                radtblinameentry.Text = dt2.Rows[0]["FLDLINAME"].ToString();
                RadRadioButtonfrequencytype.SelectedValue = dt2.Rows[0]["FLDFREQUENCYTYPE"].ToString();
                tbfrequencyentry.Text = dt2.Rows[0]["FLDFREQUENCY"].ToString();
                RadCbliunit.SelectedValue = dt2.Rows[0]["FLDLIUNIT"].ToString();
                RadRBliscope.SelectedValue = dt2.Rows[0]["FLDLISCOPE"].ToString();
                if (dt2.Rows[0]["FLDLISCOPE"].ToString() == "1")
                {
                    radcbrank.Visible = true;
                    radcbappliesto.Visible = true;
                    radlblrank.Visible = true;
                    radcbrank.HardList = PhoenixRegistersHard.ListHard(1, 51);
                    radcbrank.SelectedHard = dt2.Rows[0]["FLDDEPARTMENTORRANK"].ToString();

                    DataTable dt4 = PhoenixDashboardBSCLI.LIActionByRankList(General.GetNullableInteger(radcbrank.SelectedHard));

                    radcbappliesto.DataSource = dt4;
                    radcbappliesto.DataTextField = "FLDRANKNAME";
                    radcbappliesto.DataValueField = "FLDRANKID";
                    radcbappliesto.DataBind();

                    radcbappliesto.SelectedValue = dt2.Rows[0]["FLDACTIONBY"].ToString();
                }
                if (dt2.Rows[0]["FLDLISCOPE"].ToString() == "2")
                {
                    radlbldept.Visible = true;
                    radcbdept.Visible = true;
                    Radcombodesignationlist.Visible = true;
                    radcbdept.SelectedValue = dt2.Rows[0]["FLDDEPARTMENTORRANK"].ToString();

                    DataTable dt3 = PhoenixDashboardBSCLI.LIActionByOfficeStaffList(General.GetNullableInteger(radcbdept.SelectedValue));
                    Radcombodesignationlist.DataSource = dt3;
                    Radcombodesignationlist.DataBind();

                    Radcombodesignationlist.Value = dt2.Rows[0]["FLDACTIONBY"].ToString();

                }

                RadtblIdescriptionentry.Text = dt2.Rows[0]["FLDDESCRIPTION"].ToString();
            }


        }
    }
    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? Liid = General.GetNullableGuid(ViewState["LIID"].ToString());
                string licode = General.GetNullableString(Radliidentry.Text);
                string liname = General.GetNullableString(radtblinameentry.Text);
                int? frequency = General.GetNullableInteger(tbfrequencyentry.Text);
                string frequencytype = RadRadioButtonfrequencytype.SelectedValue;
                int? liunit = General.GetNullableInteger(RadCbliunit.SelectedValue);
                int? liscope = General.GetNullableInteger(RadRBliscope.SelectedValue);
                int? actionby = null;
                int? departmentorrank = null;
                if (RadRBliscope.SelectedValue == "2")

                {
                    departmentorrank = General.GetNullableInteger(radcbdept.SelectedValue);
                    actionby = General.GetNullableInteger(Radcombodesignationlist.Value);
                }
                else
                {
                    departmentorrank = General.GetNullableInteger(radcbrank.SelectedHard);
                    actionby = General.GetNullableInteger(radcbappliesto.SelectedValue);
                }
                string description = General.GetNullableString(RadtblIdescriptionentry.Text);
                if (!IsValidShippingLIDetails(licode, liname, liunit, liscope, frequency, frequencytype, actionby))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSCLI.LIUpdate(rowusercode, licode, liname, liunit, liscope, frequency, frequencytype, departmentorrank ,actionby, description, Liid);


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

    private bool IsValidShippingLIDetails(string licode, string liname, int? liunit, int? liscope, int? frequency, string frequencytype, int? actionby)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (licode == null)
        {
            ucError.ErrorMessage = "LI ID.";
        }
        if (liname == null)
        {
            ucError.ErrorMessage = "LI Name.";
        }
        if (liunit == null)
        {
            ucError.ErrorMessage = "LI Unit.";
        }
        if (liscope == null)
        {
            ucError.ErrorMessage = "LI Scope.";
        }
        if (frequency == null)
        {
            ucError.ErrorMessage = "LI Frequency.";
        }

        if (actionby == null)
        {
            ucError.ErrorMessage = "LI Action by.";
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

    protected void RadRBliscope_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadRBliscope.SelectedValue == "2")
        {
            radcbdept.Visible = true;
            radcbrank.Visible = false;
            radlbldept.Visible = true;
            radlblrank.Visible = false;
            radcbrank.SelectedHard = string.Empty;
            radcbappliesto.Visible = false;
            radcbappliesto.Text = string.Empty;
            radcbappliesto.ClearSelection();
            Radcombodesignationlist.Visible = true;
        }
        if (RadRBliscope.SelectedValue == "1")
        {
            radcbdept.Visible = false;
            radcbdept.ClearSelection();
            radcbdept.Text = string.Empty;
            radcbrank.Visible = true;
            radlbldept.Visible = false;
            radlblrank.Visible = true;
            radcbappliesto.Visible = true;
            Radcombodesignationlist.Visible = false;
            Radcombodesignationlist.Text = string.Empty;
            Radcombodesignationlist.Value = null;
        }
    }

    protected void radcbrank_TextChangedEvent(object sender, EventArgs e)
    {
        if (radcbrank.SelectedHard != null)
        {
            DataTable dt = PhoenixDashboardBSCLI.LIActionByRankList(General.GetNullableInteger(radcbrank.SelectedHard));

            radcbappliesto.DataSource = dt;
            radcbappliesto.DataTextField = "FLDRANKNAME";
            radcbappliesto.DataValueField = "FLDRANKID";
            radcbappliesto.DataBind();
        }
    }

    protected void radcbdept_TextChanged(object sender, EventArgs e)
    {
        DataTable dt2 = PhoenixDashboardBSCLI.LIActionByOfficeStaffList(General.GetNullableInteger(radcbdept.SelectedValue));
        Radcombodesignationlist.DataSource = dt2;
        Radcombodesignationlist.DataBind();
    }
}