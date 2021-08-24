using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDocumentLicence : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentLicence.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentLicence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentLicence.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersDocumentLicenceList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDLICENCE");
        
        MenuRegistersDocumentLicence.AccessRights = this.ViewState;
        MenuRegistersDocumentLicence.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Licence", "LICENCE");
        toolbar.AddButton("Flag List", "FLAGLIST");
        MenuLicenceCost.AccessRights = this.ViewState;
        MenuLicenceCost.MenuList = toolbar.Show();
        MenuLicenceCost.SelectedMenuIndex = 0;
        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDCATEGORYNAME", "FLDABREVATION", "FLDLICENCE", "FLDOFFICECREW", "FLDGROUP", "FLDRANKNAME", "FLDSTAGE", "FLDMANDATORYYNNAME", 
                                 "FLDWAIVERYNNAME", "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Code", "Licence", "Office Crew", "Group", "Applied To", "Offshore Stage", "Mandatory Y/N", 
                                  "Waiver Y/N", "User group to allow waiver", "'Show in 'Additional Documents' on Crew Planner Y/N'", "Requires Authentication Y/N", 
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N"  };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string LicenceSearch = (txtSearchLicence.Text == null) ? "" : txtSearchLicence.Text;

        ds = PhoenixRegistersDocumentLicence.DocumentLicenceSearch(
            null, LicenceSearch
            , chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"), null, null, null, "", null, null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentLicence.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentLicence.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Licence</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentLicence.Rebind();
    }

    protected void LicenceCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LICENCE"))
        {
            MenuLicenceCost.SelectedMenuIndex = 0;            
            return;
        }
        else if (CommandName.ToUpper().Equals("FLAGLIST"))
        {
            Response.Redirect("../Registers/RegistersLicenceFlagList.aspx");
        }
    }

    protected void RegistersDocumentLicence_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentLicence.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDCATEGORYNAME", "FLDABREVATION", "FLDLICENCE", "FLDOFFICECREW", "FLDGROUP", "FLDRANKNAME", "FLDSTAGE", "FLDMANDATORYYNNAME", 
                                 "FLDWAIVERYNNAME", "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Code", "Licence", "Office Crew", "Group", "Applied To", "Offshore Stage", "Mandatory Y/N", 
                                  "Waiver Y/N", "User group to allow waiver", "'Show in 'Additional Documents' on Crew Planner Y/N'", "Requires Authentication Y/N", 
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N"  };
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string LicenceSearch = (txtSearchLicence.Text == null) ? "" : txtSearchLicence.Text;

        DataSet ds = PhoenixRegistersDocumentLicence.DocumentLicenceSearch(
            null, LicenceSearch
            , chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"), null, null, null, "", null, null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentLicence.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentLicence", "Licence", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentLicence.DataSource = ds;
            gvDocumentLicence.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentLicence.DataSource = "";
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentLicence.Rebind();
    }
    private void DeleteDocumentLicence(int documentId)
    {
        PhoenixRegistersDocumentLicence.DeleteDocumentLicence(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, documentId);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDocumentLicence_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentId");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentLicenceList.aspx?DocumentLicenceId=" + l.Text + "');return false;");
            }
            //LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkLicense");
            //if (lbtn != null)
            //    lbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentLicenceList.aspx?DocumentLicenceId=" + l.Text + "');return false;");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkDocumentType");
            if (lb != null)
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentLicenceList.aspx?DocumentLicenceId=" + l.Text + "');return false;");


            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");
            LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImgUserGroup");
            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
            LinkButton lblRankNameList = (LinkButton)e.Item.FindControl("lblRankNameList");
            if (lblRankNameList != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDRANKNAMELIST").ToString() == string.Empty)
                    lblRankNameList.Visible = false;
            }

            LinkButton lblVesselType = (LinkButton)e.Item.FindControl("lblVesselType");
            if (lblVesselType != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDVESSELTYPE").ToString() == string.Empty)
                    lblVesselType.Visible = false;
            }
            LinkButton lblowner = (LinkButton)e.Item.FindControl("lblowner");
            if (lblowner != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDOWNERLIST").ToString() == string.Empty)
                    lblowner.Visible = false;
            }
            LinkButton lblCompany = (LinkButton)e.Item.FindControl("lblCompany");
            if (lblCompany != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPANIES").ToString() == string.Empty)
                    lblCompany.Visible = false;
            }
            LinkButton lblFlag = (LinkButton)e.Item.FindControl("lblFlag");
            if (lblFlag != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDFLAG").ToString() == string.Empty)
                    lblFlag.Visible = false;
            }
            //RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentId");

        }
    }

    protected void gvDocumentLicence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentLicence.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentLicence(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentId")).Text));
                BindData();
                gvDocumentLicence.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvDocumentLicence_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
