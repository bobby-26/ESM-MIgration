using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class HR_PayRollSGEmployeeConfigEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar menu = new PhoenixToolbar();
         
        menu.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        gvTabStrip.MenuList = menu.Show();
        if(!IsPostBack)
            Loadconfig();
    }
    protected void Loadconfig()
    {
       
        DataTable dt = PhoenixPayRollSingapore.Employeedetails(General.GetNullableInteger(Request.QueryString["employeeid"]));
        lblemployeecode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString() ;
        lblname.Text = dt.Rows[0]["FLDNAME"].ToString(); 
        ddlrace.SelectedHard = dt.Rows[0]["FLDEMPLOYEERACE"].ToString();
        radtbiaxnumber.Text = dt.Rows[0]["FLDINCOMETAXREFERENCENUMBER"].ToString();
        ddlworkertype.SelectedHard = dt.Rows[0]["FLDFOREIGNWORKERTYPE"].ToString();
        radtbwpnumber.Text = dt.Rows[0]["FLDFOREIGNWORKPERMITNUMBER"].ToString();
        radapplydate.Text = dt.Rows[0]["FLDAPPLYDATE"].ToString();
        radfromdate.Text = dt.Rows[0]["FLDWORKINGPASSEFFECTVEFROMDATE"].ToString();
        radtodate.Text = dt.Rows[0]["FLDWORKINGPASSEFFECTIVETODATE"].ToString();
        radskill.SelectedHard = dt.Rows[0]["FLDEMPLOYEESKILL"].ToString();
        radfwltier.SelectedValue = dt.Rows[0]["FLDFWLTIERID"].ToString();
        radtbspassnumber.Text = dt.Rows[0]["FLDSPASSNUMBER"].ToString();
        radtbemployeepassnumber.Text = dt.Rows[0]["FLDEMPLOYEEPASSNUMBER"].ToString();
        radtbnricno.Text = dt.Rows[0]["FLDNRICNUMBER"].ToString();
        radtbfinno.Text = dt.Rows[0]["FLDFINNUMBER"].ToString();
        radimmigrationfileno.Text = dt.Rows[0]["FLDIMMIGRATIONREFNUMBER"].ToString();
        radtbcpfaccountno.Text = dt.Rows[0]["FLDCPFACCOUNTNUMBER"].ToString();
        LoadFWLTiers(dt.Rows[0]["FLDFOREIGNWORKERTYPE"].ToString(), dt.Rows[0]["FLDEMPLOYEESKILL"].ToString());
    }

    protected void LoadFWLTiers(string workerype , string skill)
    {
        DataTable dt1 = PhoenixPayRollSingapore.FWLTierList(General.GetNullableInteger(workerype), General.GetNullableInteger(skill));
        if (dt1.Rows.Count > 0)
        {
        radfwltier.DataSource = dt1;
        radfwltier.DataValueField = "FLDSGPRFWLRATEID";
        radfwltier.DataTextField = "FLDSFPRFWLTEIR";
        radfwltier.DataBind();
        }

    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (IsValidReport())
            {
                ucError.Visible = true;
                return;
            }



            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.EmployeeConfigInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Request.QueryString["employeeid"]), General.GetNullableInteger(ddlrace.SelectedHard), General.GetNullableString(radtbiaxnumber.Text), General.GetNullableInteger(ddlworkertype.SelectedHard), General.GetNullableString(radtbwpnumber.Text), General.GetNullableString(radtbspassnumber.Text), General.GetNullableString(radtbemployeepassnumber.Text), General.GetNullableDateTime(radfromdate.Text), General.GetNullableDateTime(radtodate.Text), General.GetNullableInteger(radskill.SelectedHard), General.GetNullableDateTime(radapplydate.Text), General.GetNullableGuid(radfwltier.SelectedValue),General.GetNullableString(radtbnricno.Text),General.GetNullableString(radtbfinno.Text),General.GetNullableString(radimmigrationfileno.Text),General.GetNullableString(radtbcpfaccountno.Text));
            }



            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please provide the following ";
        if (string.IsNullOrWhiteSpace(radtbiaxnumber.Text))
        {
            ucError.ErrorMessage = "Employee Tax reference number / NRIC / FIN ";
        }
       

        if (General.GetNullableInteger(ddlworkertype.SelectedHard) != null)
        {

           

            if ((General.GetNullableDateTime(radfromdate.Text) == null))
                {
                ucError.ErrorMessage = " From date. ";

            }
            if ((General.GetNullableDateTime(radtodate.Text) == null))
            {
                ucError.ErrorMessage = " To date. ";

            }

            if (General.GetNullableDateTime(radfromdate.Text) != null && General.GetNullableDateTime(radtodate.Text) != null)
            {
                if (!(General.GetNullableDateTime(radtodate.Text) > General.GetNullableDateTime(radfromdate.Text)))
                    ucError.ErrorMessage = "To date should be greater than from date. ";
            }

            if ((General.GetNullableInteger(ddlworkertype.SelectedHard) == 1))
                {
                    radtbemployeepassnumber.Text = "";
                    radtbspassnumber.Text = "";
                if (General.GetNullableString(radtbwpnumber.Text) == null)
                {
                    ucError.ErrorMessage = "Work Permit number. ";

                }
                if ((General.GetNullableInteger(radskill.SelectedHard) == null))
                    {
                    ucError.ErrorMessage = " Skill level. ";
                    }
                }

            if ((General.GetNullableInteger(ddlworkertype.SelectedHard) == 2))
            {
                radtbemployeepassnumber.Text = "";
                radtbwpnumber.Text = "";
                if (General.GetNullableString(radtbspassnumber.Text) == null)
                {
                    ucError.ErrorMessage = "S Pass number. ";

                }

            }
            if ((General.GetNullableInteger(ddlworkertype.SelectedHard) == 3))
            {
                radtbspassnumber.Text = "";
                radtbwpnumber.Text = "";
                if (General.GetNullableString(radtbemployeepassnumber.Text) == null)
                {
                    ucError.ErrorMessage = "Employee Pass number. ";

                }
            }
        }

        return ucError.IsError;
    }

    protected void ddlworkertype_TextChangedEvent(object sender, EventArgs e)
    {
        LoadFWLTiers(General.GetNullableString(ddlworkertype.SelectedHard),General.GetNullableString(radskill.SelectedHard));
    }

    protected void radskill_TextChangedEvent(object sender, EventArgs e)
    {
        LoadFWLTiers(General.GetNullableString(ddlworkertype.SelectedHard), General.GetNullableString(radskill.SelectedHard));
    }
}