using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class CommonPickListDailyWorkPlanRAExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuPortAgent.MenuList = toolbarmain.Show();
            //MenuPortAgent.SetTrigger(pnlPortAgentEntry);             

            if (!IsPostBack)
            {
                ViewState["type"] = "0";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                gvPortAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                BindCategory();
                rblType.SelectedIndex = 0;

                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                string opt = Request.QueryString["opt"];
                if (opt != null && opt != string.Empty)
                {
                    foreach (ButtonListItem li in rblType.Items)
                        li.Enabled = false;
                    string[] str = opt.Split(',');
                    foreach (string s in str)
                    {
                        if (s.ToUpper() == "P")
                        {
                            rblType.Items[0].Enabled = true;
                            rblType.SelectedIndex = 0;
                        }
                        else if (s.ToUpper() == "G")
                        {
                            rblType.Items[1].Enabled = true;
                            rblType.SelectedIndex = 1;
                        }
                        else if (s.ToUpper() == "M")
                        {
                            rblType.Items[2].Enabled = true;
                            rblType.SelectedIndex = 2;
                        }
                        else if (s.ToUpper() == "N")
                        {
                            rblType.Items[3].Enabled = true;
                            rblType.SelectedIndex = 3;
                        }
                        else if (s.ToUpper() == "C")
                        {
                            rblType.Items[4].Enabled = true;
                            rblType.SelectedIndex = 4;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPortAgent.SelectedIndexes.Clear();
        gvPortAgent.EditIndexes.Clear();
        gvPortAgent.DataSource = null;
        gvPortAgent.Rebind();
    }
    protected void MenuPortAgent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentProcessExtn.DailyWorkRiskAssessmentExtnList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                    , General.GetNullableInteger(rblType.SelectedValue)
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , gvPortAgent.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , txtActivity.Text
                                                    , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        gvPortAgent.DataSource = ds;
        gvPortAgent.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvPortAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Add(lblRefNo.ID, lblRefNo.Text);

                if (rblType.SelectedValue == "1")
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblProcessName");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }
                else
                {
                    RadLabel lblRA = (RadLabel)e.Item.FindControl("lblActivity");
                    nvc.Add(lblRA.ID, lblRA.Text);
                }

                RadLabel lblRAId = (RadLabel)e.Item.FindControl("lblRAId");
                nvc.Add(lblRAId.ID, lblRAId.Text);
                RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
                nvc.Add(lblType.ID, lblType.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Set(nvc.GetKey(1), lblRefNo.Text);

                RadLabel lblRA = (RadLabel)e.Item.FindControl("lblActivity");
                nvc.Set(nvc.GetKey(2), lblRA.Text);

                RadLabel lblRAId = (RadLabel)e.Item.FindControl("lblRAId");
                nvc.Set(nvc.GetKey(3), lblRAId.Text);

                RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
                nvc.Set(nvc.GetKey(4), lblType.Text);

            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvPortAgent_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {            
            if (rblType.SelectedValue == "1")
            {
                e.Item.Cells[3].Visible = true;
            }
        }
    }
    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();
        if (rblType.SelectedValue == "1") //PROCESS
            ViewState["type"] = "0";
        else if (rblType.SelectedValue == "2") //GENERIC
            ViewState["type"] = "1";
        else if (rblType.SelectedValue == "3") //MACHINERY
            ViewState["type"] = "3";
        else if (rblType.SelectedValue == "4") //NAVIGATION
            ViewState["type"] = "2";
        else if (rblType.SelectedValue == "5") //CARGO
            ViewState["type"] = "4";

        BindCategory();
        gvPortAgent.Rebind();
    }

    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentActivity(General.GetNullableInteger(rblType.SelectedValue),null);

        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void gvPortAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPortAgent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
