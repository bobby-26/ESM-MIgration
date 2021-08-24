using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class RegistersDocumentOther : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentOther.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentOther')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentOther.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersDocumentOtherAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuRegistersDocumentOther.AccessRights = this.ViewState;
        MenuRegistersDocumentOther.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = "FLDGROUP";
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDocumentOther.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDOCUMENTCODE", "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDLOCALACTIVEYN", "FLDGROUPNAME", "FLDHAVINGEXPIRYYN", "FLDSHORTEXPIRYYN", "FLDNOKYN", "FLDSTAGE",
                                 "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYNNAME", "FLDPHOTOCOPYACCEPTABLEYNNAME" };
        string[] alCaptions = { "Document Code", "Document Category", "Document Name", "Active Y/N", "Group", "Having Expiry", "Short Expiry", "Nok Y/N", "Offshore Stage", "Mandatory Y/N",
                                  "Waiver Y/N", "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N", "Show in Master's checklist onboard Y/N", "Photocopy Acceptable Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string OtherDocumentsSearch = (txtSearchOtherDocuments.Text == null) ? "" : txtSearchOtherDocuments.Text;

        ds = PhoenixRegistersDocumentOther.DocumentOtherSearch(OtherDocumentsSearch, chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"), null, null, null, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentOther.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentOther.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Other Document</h3></td>");
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

    protected void RegistersDocumentOther_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentOther.Rebind();
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

        string[] alColumns = { "FLDDOCUMENTCODE", "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDLOCALACTIVEYN", "FLDGROUPNAME", "FLDHAVINGEXPIRYYN", "FLDSHORTEXPIRYYN", "FLDNOKYN", "FLDSTAGE",
                                 "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYNNAME", "FLDPHOTOCOPYACCEPTABLEYNNAME" };
        string[] alCaptions = { "Document Code", "Document Category", "Document Name", "Active Y/N", "Group", "Having Expiry", "Short Expiry", "Nok Y/N", "Offshore Stage", "Mandatory Y/N",
                                  "Waiver Y/N", "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N", "Show in Master's checklist onboard Y/N", "Photocopy Acceptable Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string OtherDocumentsSearch = (txtSearchOtherDocuments.Text == null) ? "" : txtSearchOtherDocuments.Text;

        DataSet ds = PhoenixRegistersDocumentOther.DocumentOtherSearch(OtherDocumentsSearch, chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"), null, null, null, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentOther.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentOther", "Other Document", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentOther.DataSource = ds;
            gvDocumentOther.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentOther.DataSource = "";
        }
    }
    
    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentOther.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        //ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkMandatoryYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;        
        GridDataItem Item = (GridDataItem)cb.NamingContainer;
        //int rowindex = 0;
        //if (ViewState["ROWINDEX"] != null)
        //    rowindex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value;
        // GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        RadCheckBox chkWaiverYNEdit = (RadCheckBox)Item.FindControl("chkWaiverYNEdit");
        RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)Item.FindControl("chkUserGroupEdit");
        if (chkWaiverYNEdit != null)
        {
            if (cb.Checked == true)
            {
                chkWaiverYNEdit.Enabled = true;
            }
            else
            {
                chkWaiverYNEdit.Checked = false;
                chkWaiverYNEdit.Enabled = false;

                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkMandatoryYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)cb.NamingContainer;
        //RadCheckBox cb = (RadCheckBox)sender;
        //int rowindex = 0;
        //if (ViewState["ROWINDEX"] != null)
        //    rowindex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value;
        RadCheckBox chkWaiverYNAdd = (RadCheckBox)Item.FindControl("chkWaiverYNAdd");
        RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)Item.FindControl("chkUserGroupAdd");
        if (chkWaiverYNAdd != null)
        {
            if (cb.Checked == true)
                chkWaiverYNAdd.Enabled = true;
            else
            {
                chkWaiverYNAdd.Checked = false;
                chkWaiverYNAdd.Enabled = false;

                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridDataItem Item = (GridDataItem)cb.NamingContainer;
        //GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;
        //int rowindex = 0;
        //if (ViewState["ROWINDEX"] != null)
        //    rowindex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value;
        RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)Item.FindControl("chkUserGroupEdit");
        if (chkUserGroupEdit != null)
        {
            if (cb.Checked == true)
            {
                chkUserGroupEdit.Enabled = true;
            }
            else
            {
                chkUserGroupEdit.SelectedIndex = -1;
                chkUserGroupEdit.Enabled = false;
            }
        }
    }

    protected void chkWaiverYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        GridFooterItem Item = (GridFooterItem)cb.NamingContainer;
        //int rowindex = 0;
        //if (ViewState["ROWINDEX"] != null)
        //    rowindex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value;
        RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)Item.FindControl("chkUserGroupAdd");

        if (chkUserGroupAdd != null)
        {
            if (cb.Checked == true)
            {
                chkUserGroupAdd.Enabled = true;
            }
            else
            {
                chkUserGroupAdd.SelectedIndex = -1;
                chkUserGroupAdd.Enabled = false;
            }
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentOther.Rebind();
    }
    private void InsertDocumentOther(string documentcode, string documentname, int? localactive, int? group, int? havingexpiry, int? ShortExpiry, int? nokyn
            , int? stage, int? offshoremandatory, int? waiver, string waiverusergroup, int? categoryid, int? additionDocYN, int? authenticationYN, int? CBAOtherDocYN
            , int? showinmasterchecklistyn, int? photocopyacceptableyn)
    {
        if (!IsValidDocumentOther(documentname, waiver, waiverusergroup))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentOther.InsertDocumentOther(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentcode, documentname, localactive, group, havingexpiry, ShortExpiry, nokyn, stage, offshoremandatory, waiver, waiverusergroup, additionDocYN, authenticationYN, categoryid, CBAOtherDocYN
            , showinmasterchecklistyn, photocopyacceptableyn);
    }

    private void UpdateDocumentOther(int documentid, string documentcode, string documentname, int? localactive, int? group, int? havingexpiry, int? ShortExpiry, int? nokyn
          , int? stage, int? offshoremandatory, int? waiver, string waiverusergroup, int? categoryid, int? additionDocYN, int? authenticationYN, int? CBAOtherDocYN
          , int? showinmasterchecklistyn, int? photocopyacceptableyn)
    {
        if (!IsValidDocumentOther(documentname, waiver, waiverusergroup))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentOther.UpdateDocumentOther(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentid, documentcode, documentname, localactive, group, havingexpiry, ShortExpiry, nokyn, stage, offshoremandatory, waiver, waiverusergroup, additionDocYN, authenticationYN, categoryid, CBAOtherDocYN
            , showinmasterchecklistyn, photocopyacceptableyn);
    }

    private bool IsValidDocumentOther(string documentname, int? waiveryn, string usergroup)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (documentname.Trim().Equals(""))
            ucError.ErrorMessage = "Document Name name required.";

        if (waiveryn != null && waiveryn.Equals("1"))
        {
            if (General.GetNullableInteger(usergroup) == null)
                ucError.ErrorMessage = "User group is required.";
        }

        return (!ucError.IsError);
    }

    private void DeleteDocumentOther(int documentid)
    {
        PhoenixRegistersDocumentOther.DeleteDocumentOther(
        PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , documentid);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDocumentOther_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadCheckBoxList chka = (RadCheckBoxList)e.Item.FindControl("chkUserGroupAdd");
                string UGList = "";
                string UserGroupList = "";
                foreach (ButtonListItem li in chka.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                InsertDocumentOther(
                    ((RadTextBox)e.Item.FindControl("txtDocumentCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDocumentNameAdd")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkLocalActiveAdd")).Checked == true) ? 1 : 0,
                    General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucGroupAdd")).SelectedHard),
                    (((RadCheckBox)e.Item.FindControl("chkHavingExpiryAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkShortExpiryAdd")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkNokYnAdd")).Checked == true) ? 1 : 0,
                     General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlStageAdd")).SelectedValue),
                     General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkMandatoryYNAdd")).Checked == true ? "1" : "0"),
                     General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkWaiverYNAdd")).Checked == true ? "1" : "0"),
                    General.GetNullableString(UserGroupList),
                    General.GetNullableInteger(((UserControlDocumentCategory)e.Item.FindControl("ucCategoryAdd")).SelectedDocumentCategoryID),
                    (((RadCheckBox)e.Item.FindControl("chkAdditionDocYnAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkAuthReqYnAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkCBAOtherDocumentAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkShowInMasterChecklistYNAdd")).Checked == true) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkPhotocopyAcceptableYnAdd")).Checked == true) ? 1 : 0
                );
                BindData();
                gvDocumentOther.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                UpdateDocumentOther(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtDocumentCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDocumentNameEdit")).Text,
                     (((RadCheckBox)e.Item.FindControl("chkLocalActiveEdit")).Checked == true) ? 1 : 0,
                     General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucGroupEdit")).SelectedHard),
                     (((RadCheckBox)e.Item.FindControl("chkHavingExpiryEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkShortExpiryEdit")).Checked == true) ? 1 : 0,
                      (((RadCheckBox)e.Item.FindControl("chkNokYnEdit")).Checked == true) ? 1 : 0,
                      General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlStageEdit")).SelectedValue),
                      General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit")).Checked == true ? "1" : "0"),
                      General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkWaiverYNEdit")).Checked == true ? "1" : "0"),
                      General.GetNullableString(UserGroupList),
                      General.GetNullableInteger(((UserControlDocumentCategory)e.Item.FindControl("ucCategoryEdit")).SelectedDocumentCategoryID),
                      General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkAdditionDocYnEdit")).Checked == true ? "1" : "0"),
                      General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkAuthReqYnEdit")).Checked == true ? "1" : "0"),
                      General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkCBAOtherDocumentEdit")).Checked == true ? "1" : "0"),
                      (((RadCheckBox)e.Item.FindControl("chkShowInMasterChecklistYNEdit")).Checked == true) ? 1 : 0,
                      (((RadCheckBox)e.Item.FindControl("chkPhotocopyAcceptableYnEdit")).Checked == true) ? 1 : 0
                );

                BindData();
                gvDocumentOther.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentOther(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentId")).Text));
                BindData();
                gvDocumentOther.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["ROWINDEX"] = e.Item.RowIndex;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
                string UGList = "";
                string UserGroupList = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        UGList += li.Value + ",";
                    }
                }

                if (UGList != "")
                {
                    UserGroupList = "," + UGList;
                }

                UpdateDocumentOther(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtDocumentCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDocumentNameEdit")).Text,
                     (((RadCheckBox)e.Item.FindControl("chkLocalActiveEdit")).Checked == true) ? 1 : 0,
                     General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucGroupEdit")).SelectedHard),
                     (((RadCheckBox)e.Item.FindControl("chkHavingExpiryEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkShortExpiryEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkNokYnEdit")).Checked == true) ? 1 : 0,
                     General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlStageEdit")).SelectedValue),
                     General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit")).Checked == true ? "1" : "0"),
                     General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkWaiverYNEdit")).Checked == true ? "1" : "0"),
                     General.GetNullableString(UserGroupList),
                     General.GetNullableInteger(((UserControlDocumentCategory)e.Item.FindControl("ucCategoryEdit")).SelectedDocumentCategoryID),
                     (((RadCheckBox)e.Item.FindControl("chkAdditionDocYnEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkAuthReqYnEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkCBAOtherDocumentEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkShowInMasterChecklistYNEdit")).Checked == true) ? 1 : 0,
                     (((RadCheckBox)e.Item.FindControl("chkPhotocopyAcceptableYnEdit")).Checked == true) ? 1 : 0
                 );
                BindData();
                gvDocumentOther.Rebind();
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

    protected void gvDocumentOther_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentOther.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentOther_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentId");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentOtherAdd.aspx?OtherId=" + l.Text + "');return false;");
            }
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkDocumentName");
            if (lbtn != null)
                lbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentOtherAdd.aspx?OtherId=" + l.Text + "');return false;");


            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucGroupEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDGROUP"].ToString();

            RadComboBox ddlStageEdit = (RadComboBox)e.Item.FindControl("ddlStageEdit");
            if (ddlStageEdit != null)
            {
                BindOffshoreStages(ddlStageEdit);
                ddlStageEdit.SelectedValue = drvHard["FLDSTAGEID"].ToString();
            }
            RadCheckBoxList chkUserGroupEdit = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
            RadCheckBox chkMandatoryYNEdit = (RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit");
            RadCheckBox chkWaiverYNEdit = (RadCheckBox)e.Item.FindControl("chkWaiverYNEdit");
            if (chkMandatoryYNEdit != null)
            {
                if (chkMandatoryYNEdit.Checked == true)
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = true;
                }
                else
                {
                    if (chkWaiverYNEdit != null) chkWaiverYNEdit.Enabled = false;
                }
            }

            if (chkWaiverYNEdit != null)
            {
                if (chkWaiverYNEdit.Checked == true)
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = true;
                }
                else
                {
                    if (chkUserGroupEdit != null) chkUserGroupEdit.Enabled = false;
                }
            }

            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = SessionUtil.UserGroupList();
                chkUserGroupEdit.DataBindings.DataTextField = "FLDGROUPNAME";
                chkUserGroupEdit.DataBindings.DataValueField = "FLDGROUPCODE";
                chkUserGroupEdit.DataBind();

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("chkUserGroupEdit");
                foreach (ButtonListItem li in chk.Items)
                {
                    string[] slist = drv["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            UserControlDocumentCategory ucCategory = (UserControlDocumentCategory)e.Item.FindControl("ucCategoryEdit");
            if (ucCategory != null)
            {
                ucCategory.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategory.DataBind();
                if (General.GetNullableInteger(drv["FLDDOCUMENTCATEGORYID"].ToString()) != null)
                    ucCategory.SelectedDocumentCategoryID = drv["FLDDOCUMENTCATEGORYID"].ToString();
            }
            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblUserGroup.ClientID;
            }
            //LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImgUserGroup");

            //if (ImgUserGroup != null)
            //{
            //    if (lblUserGroup != null)
            //    {
            //        if (lblUserGroup.Text != "")
            //        {
            //            ImgUserGroup.Visible = true;
            //            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
            //            if (uct != null)
            //            {
            //                ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //                ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            //            }
            //        }
            //        else
            //            ImgUserGroup.Visible = false;
            //    }
            //}

            LinkButton lblRankNameList = (LinkButton)e.Item.FindControl("lblRankNameList");
            if (lblRankNameList != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDRANKNAME").ToString() == string.Empty)
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
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            RadComboBox ddlStageAdd = (RadComboBox)e.Item.FindControl("ddlStageAdd");
            if (ddlStageAdd != null)
            {
                BindOffshoreStages(ddlStageAdd);
            }

            RadCheckBoxList chkUserGroupAdd = (RadCheckBoxList)e.Item.FindControl("chkUserGroupAdd");
            if (chkUserGroupAdd != null)
            {
                chkUserGroupAdd.DataSource = SessionUtil.UserGroupList();
                chkUserGroupAdd.DataBindings.DataTextField = "FLDGROUPNAME";
                chkUserGroupAdd.DataBindings.DataValueField = "FLDGROUPCODE";
                chkUserGroupAdd.DataBind();
            }
            UserControlDocumentCategory ucCategoryAdd = (UserControlDocumentCategory)e.Item.FindControl("ucCategoryAdd");
            if (ucCategoryAdd != null)
            {
                ucCategoryAdd.DocumentCategoryList = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null
                    , null
                    );
                ucCategoryAdd.DataBind();
            }
        }
    }

    protected void gvDocumentOther_SortCommand(object sender, GridSortCommandEventArgs e)
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
           