using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewTravelPlanBreakupCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Copy", "COPYBREAKUP");
            MenuBreakupCopy.AccessRights = this.ViewState;
            MenuBreakupCopy.MenuList = toolbar.Show();
              
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                    ViewState["Requestid"] = Request.QueryString["REQUESTID"].ToString();
                }
            }
            BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBreakupCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper() == "COPYBREAKUP")
            {
                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strtravelrequestlist = new StringBuilder();

                foreach (GridViewRow gvr in gvPlan.Rows)
                {
                    CheckBox chkCopy = (CheckBox)gvr.FindControl("chkCopy");

                    if (chkCopy.Checked)
                    {
                        Label lblEmployeeId = (Label)gvr.FindControl("lblEmployeeId");
                        Label lblPlanId = (Label)gvr.FindControl("lblRequestId");

                        stremployeelist.Append(lblEmployeeId.Text);
                        stremployeelist.Append(",");

                        strtravelrequestlist.Append(lblPlanId.Text);
                        strtravelrequestlist.Append(",");
                    }
                }

                if (stremployeelist.Length > 1)
                {
                    stremployeelist.Remove(stremployeelist.Length - 1, 1);
                }
                else
                {
                    ucError.ErrorMessage = "Please select Employees to copy the breakup";
                    ucError.Visible = true;
                    return;
                }

                if (strtravelrequestlist.Length > 1)
                {
                    strtravelrequestlist.Remove(strtravelrequestlist.Length - 1, 1);
                }


                PhoenixCrewTravelRequest.CopyTravelPlanBreakUp(stremployeelist.ToString()
                    , strtravelrequestlist.ToString()
                    , new Guid(ViewState["Requestid"].ToString())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString()));

                ucStatus.Text = "Employee Breakup details copied successfully.";
                BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindData()
    {
        try
        {

            DataTable dt = PhoenixCrewTravelRequest.CrewPlanBreakupList(General.GetNullableInteger(ViewState["Vesselid"].ToString()), General.GetNullableGuid(ViewState["Requestid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvPlan.DataSource = dt;
                gvPlan.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPlan);
            }

       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}