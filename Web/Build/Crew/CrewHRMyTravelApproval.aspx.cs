using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
public partial class CrewHRMyTravelApproval : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["APP"] = null;

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if(Request.QueryString["App"] != null && Request.QueryString["App"].ToString() != "")
                {
                    ViewState["APP"] = Request.QueryString["App"].ToString();
                }
                
                CheckWebSessionStatus();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (Request.QueryString["App"] != null && ViewState["APP"].ToString() == "1")
            {
                toolbarmain.AddButton("Approve", "APPROVE",ToolBarDirection.Right);
            }
            else
            {
                toolbarmain.AddButton("Reject", "REJECT",ToolBarDirection.Right);
            }

            MenuTravelPassengerMain.AccessRights = this.ViewState;
            MenuTravelPassengerMain.MenuList = toolbarmain.Show();

            if (ViewState["USERCODE"] != null && ViewState["USERCODE"].ToString() == "")
            {
                ucError.ErrorMessage = "You need a valid Phoenix username if you are an Office Staff. Provide a valid Phoenix username.";
                ucError.Visible = true;
                return;
            }
            Edit();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Edit()
    {
        try
        {
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelApprovalEDIT(new Guid(ViewState["TRAVELREQUESTID"].ToString()), int.Parse(ViewState["USERCODE"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["TITLE"] = dt.Rows[0]["FLDTRAVELREQUESTNO"].ToString();
                txtRequestno.Text = dt.Rows[0]["FLDTRAVELREQUESTNO"].ToString();
                txtRequestedemail.Text = dt.Rows[0]["FLDSTAFFMAIL"].ToString();
                txtRequestname.Text = dt.Rows[0]["FLDREGARDSNAME"].ToString();
                txtLevel.Text = dt.Rows[0]["FLDLEVEL"].ToString();
                txtapprover.Text = dt.Rows[0]["FLDAPPROVERNAME"].ToString();
                ViewState["STAFFMAIL"] = dt.Rows[0]["FLDSTAFFMAIL"].ToString();
                ViewState["APPROVERID"] = dt.Rows[0]["FLDAPPROVERID"].ToString();
                ViewState["LEVEL"] = dt.Rows[0]["FLDLEVEL"].ToString();
                ViewState["TYPEID"] = dt.Rows[0]["FLDTYPEID"].ToString();
                ViewState["PERSONALINFOSN"] = dt.Rows[0]["FLDPERSONALINFOSN"].ToString();
                ViewState["STAFFNAME"] = dt.Rows[0]["FLDREGARDSNAME"].ToString();
                ViewState["APPROVERNAME"] = dt.Rows[0]["FLDAPPROVERNAME"].ToString();
                ViewState["STAFFSALUTATION"] = dt.Rows[0]["FLDREGARDSALUTATION"].ToString();
                
                PhoenixToolbar toolbarmenu = new PhoenixToolbar();
                MenuTab.AccessRights = this.ViewState;
                MenuTab.Title = dt.Rows[0]["FLDTRAVELREQUESTNO"].ToString();
                MenuTab.MenuList = toolbarmenu.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCrewHRTravelRequest.HRTravelApprovalsessionEdit(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["APPROVERID"] = dt.Rows[0]["FLDONBEHALF"].ToString();
            ViewState["USERCODE"] = dt.Rows[0]["FLDUSERID"].ToString();
            ViewState["TRAVELREQUESTID"] = dt.Rows[0]["FLDAPPROVALID"].ToString();
            ViewState["SESSIONID"] = dt.Rows[0]["FLDDTKEY"].ToString();
            txtremarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        }
    }

    protected void TravelPassengerMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (!IsValidRemarks(txtremarks.Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewHRTravelRequest.HRTravelApprovalConfiguration(int.Parse(ViewState["USERCODE"].ToString())
                                                                         , new Guid(ViewState["TRAVELREQUESTID"].ToString())
                                                                         , int.Parse(ViewState["APPROVERID"].ToString())
                                                                         , txtremarks.Text.Trim()
                                                                         , int.Parse(ViewState["LEVEL"].ToString())
                                                                         , int.Parse(ViewState["TYPEID"].ToString())
                                                                         , new Guid(ViewState["SESSIONID"].ToString()), 1);
                SendForApproval();
                SendApproved();
                BindBreakUpData();
                gvTravelRequestBreakup.Rebind();
                BindData();
                gvTravelPassenger.Rebind();

                String script = String.Format("javascript:parent.fnReloadList('approval');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ucStatus.Text = "Successfully Approved.";

            }
            if (CommandName.ToUpper().Equals("REJECT"))
            {
                if (!IsValidRemarks(txtremarks.Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewHRTravelRequest.HRTravelApprovalConfiguration(int.Parse(ViewState["USERCODE"].ToString())
                                                                         , new Guid(ViewState["TRAVELREQUESTID"].ToString())
                                                                         , int.Parse(ViewState["APPROVERID"].ToString())
                                                                         , txtremarks.Text.Trim()
                                                                         , int.Parse(ViewState["LEVEL"].ToString())
                                                                         , int.Parse(ViewState["TYPEID"].ToString())
                                                                         , new Guid(ViewState["SESSIONID"].ToString()), 0);
                SendApproved();
                BindBreakUpData();               
                gvTravelRequestBreakup.Rebind();
                BindData();
                gvTravelPassenger.Rebind();

                String script = String.Format("javascript:parent.fnReloadList('approval');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ucStatus.Text = "Successfully Rejected.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemarks(string Remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Remarks == "")
            ucError.ErrorMessage = "Remarks is  required";

        return (!ucError.IsError);
    }

    private void BindBreakUpData()
    {
        try
        {
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelRequestBreakUpSearch(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString())
                , int.Parse(ViewState["USERCODE"].ToString()));

            gvTravelRequestBreakup.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelRequestBreakup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindBreakUpData();
    }

    protected void gvTravelRequestBreakup_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvTravelRequestBreakup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

        }

    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDDATEOFBIRTH" };
            string[] alCaptions = { "S.No.", "Name", "DOB" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewHRTravelRequest.HRTravelPassengerSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString())
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvTravelPassenger.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount, int.Parse(ViewState["USERCODE"].ToString()));

            General.SetPrintOptions("gvTravelPassenger", "Travel Passenger List", alCaptions, alColumns, ds);

            gvTravelPassenger.DataSource = ds;
            gvTravelPassenger.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvTravelPassenger_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelPassenger.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvTravelPassenger_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelPassenger_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }


    private void SendApproved()
    {
        string emailid;
        try
        {
            string emailbodytext = "";

            emailid = ViewState["STAFFMAIL"].ToString();

            try
            {
                emailbodytext = PrepareEmailBodyText(ViewState["APPROVERNAME"].ToString(), ViewState["STAFFSALUTATION"].ToString() + ' ' + ViewState["STAFFNAME"].ToString());
                PhoenixCommoneProcessing.PrepareEmailMessage(emailid, "TRAVEL", new Guid(ViewState["SESSIONID"].ToString()), "", "", "", ViewState["APP"].ToString() == "1" ? "Travel request is approved" : "Travel request is rejected ", emailbodytext, "", "");

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendForApproval()
    {
        string emailid;
        try
        {
            int i = 0;
            DataTable dt = PhoenixCrewHRTravelRequest.HRTravelApprovaluserlist(new Guid(ViewState["TRAVELREQUESTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (i == 0)
                    {
                        string emailbodytext = "";
                        emailid = dr["APPROVALMAIL"].ToString();
                        try
                        {
                            {
                                emailbodytext = PrepareEmailBody(new Guid(dr["FLDDTKEY"].ToString()), dr["FLDAPPROVERSALUTATION"].ToString() + ' ' + dr["FLDAPPROVERNAME"].ToString(), dr["FLDREGARDSNAME"].ToString());
                                PhoenixCommoneProcessing.PrepareEmailMessage(emailid, "TRAVEL", new Guid(dr["FLDDTKEY"].ToString()), "", "", "", "MY TRAVEL REQUEST", emailbodytext, "", "");
                            }
                        }
                        catch (Exception ex)
                        { ucError.ErrorMessage = ex.Message; }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string PrepareEmailBody(Guid quotationid, string approvername, string regards)
    {
        StringBuilder sbemailbody = new StringBuilder();
        try
        {
            sbemailbody.AppendLine();
            sbemailbody.Append("Dear " + approvername + ",");
            sbemailbody.AppendLine("\n Please find my below travel request:");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Approve");
            sbemailbody.AppendLine("" + "\"<" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?sessionid=" + quotationid.ToString() + "&App=1" + ">\"");
            sbemailbody.AppendLine("Reject");
            sbemailbody.AppendLine("" + "\"<" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?sessionid=" + quotationid.ToString() + "&App=0" + ">\"");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine(" I request your confirmation/ approval to go ahead with this travel.");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("Regards");
            sbemailbody.AppendLine(regards);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return sbemailbody.ToString();
    }
    protected string PrepareEmailBodyText(string approvername, string regards)
    {
        StringBuilder sbemailbody = new StringBuilder();
        try
        {
            if (ViewState["APP"].ToString() == "1")
            {
                DataTable dt = PhoenixCrewHRTravelRequest.HRTravelApprovaluserlist(new Guid(ViewState["TRAVELREQUESTID"].ToString()));
                sbemailbody.AppendLine();
                sbemailbody.Append("Dear " + regards);
                sbemailbody.AppendLine("\n Your Travel Request No. " + ViewState["TITLE"].ToString() + "  is approved.");
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Remarks :" + txtremarks.Text);
                sbemailbody.AppendLine();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sbemailbody.AppendLine("\n" + dr["FLDAPPROVERSALUTATION"].ToString() + ' ' + dr["FLDAPPROVERNAME"].ToString() + " : " + dr["FLDSTATUSDESC"].ToString());
                    }
                }

                sbemailbody.AppendLine(" ");
                sbemailbody.AppendLine("Regards");
                sbemailbody.AppendLine(approvername);
            }
            if (ViewState["APP"].ToString() == "0")
            {
                sbemailbody.AppendLine();
                sbemailbody.Append("Dear " + regards);
                sbemailbody.AppendLine("\n Your Travel Request No. " + ViewState["TITLE"].ToString() + "  is rejected.");
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Remarks :" + txtremarks.Text);
                sbemailbody.AppendLine(" ");
                sbemailbody.AppendLine("Regards");
                sbemailbody.AppendLine(approvername);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return sbemailbody.ToString();
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvTravelPassenger.Rebind();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }


}