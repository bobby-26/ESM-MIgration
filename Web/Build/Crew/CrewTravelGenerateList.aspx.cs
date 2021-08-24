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
using System.Web;

public partial class Crew_CrewTravelGenerateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Raise Travel", "GENERATETRAVEL");
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["PURPOSEID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                  
                    ViewState["PurposeId"] = Request.QueryString["PURPOSEID"].ToString();
                }
                if (Request.QueryString["PORTID"] != null)
                {
                    ViewState["Port"] = Request.QueryString["PORTID"].ToString();
                }
                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["Requestid"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["VESSELACCOUNTID"] != null)
                {
                    ViewState["VesselAccount"] = Request.QueryString["VESSELACCOUNTID"].ToString();
                }
                if (Request.QueryString["CREWCHANGEDATE"] != null)
                {
                    ViewState["CrewChangeDate"] = Request.QueryString["CREWCHANGEDATE"].ToString();
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

    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper() == "GENERATETRAVEL")
            {


                if (rbGenerateType.SelectedValue == "2")
                {
                    string StrTravelId;

                    if (gvTravel.SelectedIndex > -1)
                    {
                        StrTravelId = ((Label)gvTravel.Rows[gvTravel.SelectedIndex].FindControl("lblTravelId")).Text;

                        Guid? EmailTravelid = null;

                        PhoenixCrewTravelRequest.InsertTravelPlanRequest(int.Parse(ViewState["Vesselid"].ToString()), General.GetNullableGuid(ViewState["Requestid"].ToString()), General.GetNullableInteger(ViewState["Port"].ToString())
                                                                         , General.GetNullableDateTime(ViewState["CrewChangeDate"].ToString())
                                                                         , General.GetNullableInteger(ViewState["PurposeId"].ToString())
                                                                         , General.GetNullableInteger(ViewState["VesselAccount"].ToString())
                                                                         , General.GetNullableGuid(StrTravelId)
                                                                         , ref EmailTravelid);
                        SendMail(EmailTravelid);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo',null);", true);

                    }
                    else
                    {
                        ucError.ErrorMessage = "Select any Existing Travel";
                        ucError.Visible = true;
                    }
                }
                else
                {
                    Guid? EmailTravelid = null;

                    PhoenixCrewTravelRequest.InsertTravelPlanRequest(int.Parse(ViewState["Vesselid"].ToString()), General.GetNullableGuid(ViewState["Requestid"].ToString()), General.GetNullableInteger(ViewState["Port"].ToString())
                                                                         , General.GetNullableDateTime(ViewState["CrewChangeDate"].ToString())
                                                                         , General.GetNullableInteger(ViewState["PurposeId"].ToString())
                                                                         , General.GetNullableInteger(ViewState["VesselAccount"].ToString())
                                                                         , null
                                                                         , ref EmailTravelid);
                    SendMail(EmailTravelid);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo',null);", true);
                }

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

    protected string PrepareEmailBodyText(string formno, string sendto, string username, string passengers)
    {

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.Append("Dear  " + sendto + " ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby inform you that, travel Request[Requisition NO : " + formno + "] has been intiated for the below crew members");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(passengers + "<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();
    }

    private void SendMail(Guid? Crewtravelid)
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelRequest.travelrequestmailsearch(Crewtravelid);
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareEmailBodyText(dr["FLDREQUISITIONNO"].ToString(), dr["FLDSENDTO"].ToString()
                                                    , dr["FLDSENDBY"].ToString(), dr["FLDPASSENGERS"].ToString());

                PhoenixMail.SendMail(ds.Tables[0].Rows[0]["FLDTRAVELPICEMAIL"].ToString(), ds.Tables[0].Rows[0]["FLDEMAIL2"].ToString().TrimEnd(','), null, ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString()
                                            , emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void BindData()
    {
        try
        {

            DataTable dt = PhoenixCrewTravelRequest.CrewTravelGenerateList(int.Parse(ViewState["Vesselid"].ToString()), int.Parse(ViewState["PurposeId"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvTravel.DataSource = dt;
                gvTravel.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvTravel);
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