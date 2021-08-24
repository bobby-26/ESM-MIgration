using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMOCApproverUserList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
        MenuUser.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["MOCID"] = "";
            ViewState["APPROVERROLEID"] = "";
            if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != "")
            {
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
            }
            BindMOCApproverRole();
            BindMOCDesignation();
        }
    }

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
        BindData();

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

        ds = PhoenixInspectionMOCCategory.MOCProposalApprovalUserSearch(
                       General.GetNullableInteger(ddlDesignation.SelectedValue),
                       General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvUser.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);

        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();

                RadLabel lblFirstName = (RadLabel)e.Item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)e.Item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)e.Item.FindControl("lblLastName");
                nvc.Add(lblFirstName.ID, lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)e.Item.FindControl("lblDesignation");
                nvc.Add(lblLastName.ID, lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)e.Item.FindControl("lblUserCode");
                nvc.Add(lblUserCode.ID, lblUserCode.Text);
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblFirstName = (RadLabel)e.Item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)e.Item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)e.Item.FindControl("lblLastName");
                nvc.Set(nvc.GetKey(1), lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)e.Item.FindControl("lblDesignation");
                nvc.Set(nvc.GetKey(2), lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)e.Item.FindControl("lblUserCode");
                nvc.Set(nvc.GetKey(3), lblUserCode.Text);
                RadLabel lblEmail = (RadLabel)e.Item.FindControl("lblEmail");
                nvc.Set(nvc.GetKey(4), lblEmail.Text);


            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvUser_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvUser_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    }
    protected void BindMOCDesignation()
    {
        ddlDesignation.DataSource = PhoenixInspectionMOCCategory.MOCProposalApprovalUserDesignationList(General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()));
        ddlDesignation.DataTextField = "FLDDESIGNATIONNAME";
        ddlDesignation.DataValueField = "FLDDESIGNATIONID";
        ddlDesignation.DataBind();
        ddlDesignation.Items.Insert(0, new RadComboBoxItem("All", ""));
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindMOCApproverRole()
    {
        DataSet ds;
        ds = PhoenixInspectionMOCRequestForChange.MOCRequestEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["MOCID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow d = ds.Tables[0].Rows[0];
            ViewState["APPROVERROLEID"] = d["FLDPERMANENTAPPROVERROLEID"].ToString();
        }        
        DataTable dt;
        dt = PhoenixInspectionMOCApproverRole.MOCApproverRoleedit(General.GetNullableGuid(ViewState["APPROVERROLEID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtRole.Text = dr["FLDMOCAPPROVERROLE"].ToString();
        }
    }
    protected void gvUser_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUser.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

