using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMiscellaneousAppraisalOccasion : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersMiscellaneousAppraisalOccasion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('dgMiscellaneousAppraisalOccasion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Registers/RegistersMiscellaneousAppraisalOccasion.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersMiscellaneousAppraisalOccasion.AccessRights = this.ViewState;
            MenuRegistersMiscellaneousAppraisalOccasion.MenuList = toolbar1.Show();
            toolbar1 = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                dgMiscellaneousAppraisalOccasion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDOCCASION", "FLDAPPRAISALONSIGNOFF", "FLDOCCASIONACTIVEYN" };
        string[] alCaptions = { "Rank Group", "Occasion", "Appraisal on Signoff", "Active Y/N" };

        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        

        DataSet ds = PhoenixRegistersMiscellaneousAppraisalOccasion.MiscellaneousAppraisalOccasionSearch(
                                                                                             General.GetNullableInteger(ucCategory.SelectedHard)
                                                                                           , General.GetNullableInteger(ucOccasion.SelectedOccassionId)
                                                                                           , sortdirection
                                                                                           , (int)ViewState["PAGENUMBER"]
                                                                                           , dgMiscellaneousAppraisalOccasion.PageSize
                                                                                           , ref iRowCount
                                                                                           , ref iTotalPageCount
                                                                                        );

        General.SetPrintOptions("dgMiscellaneousAppraisalOccasion", "Rank Group Mapping", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgMiscellaneousAppraisalOccasion.DataSource = ds;
            dgMiscellaneousAppraisalOccasion.VirtualItemCount = iRowCount;
        }
        else
        {
            dgMiscellaneousAppraisalOccasion.DataSource = "";
        }
    }    

    protected void RegistersMiscellaneousAppraisalOccasion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;                
                BindData();
                dgMiscellaneousAppraisalOccasion.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucOccasion.SelectedOccassionId= "";
                ucCategory.SelectedHard = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                dgMiscellaneousAppraisalOccasion.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDOCCASION",  "FLDAPPRAISALONSIGNOFF", "FLDOCCASIONACTIVEYN" };
        string[] alCaptions = { "Rank Group", "Occasion", "Appraisal on Signoff" ,"Active Y/N"}; 
       
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousAppraisalOccasion.MiscellaneousAppraisalOccasionSearch(
                                                                                            General.GetNullableInteger(ucCategory.SelectedHard)                                                                                        
                                                                                          , General.GetNullableInteger(ucOccasion.SelectedOccassionId)
                                                                                          , sortdirection
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                          , dgMiscellaneousAppraisalOccasion.PageSize
                                                                                          , ref iRowCount
                                                                                          , ref iTotalPageCount
                                                                                       );


        Response.AddHeader("Content-Disposition", "attachment; filename=RankGroupMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rank Group Mapping</h3></td>");
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
    private bool IsValidAppraisalOccasion(string occassion, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (occassion.Trim().Equals(""))
            ucError.ErrorMessage = "Occasion is required.";

        if (General.GetNullableInteger(category) == null)
            ucError.ErrorMessage = "Category is required.";
        return (!ucError.IsError);
    }

    private void InsertAppraisalOccasion(string occassion, int signoff, int category, int activeyn , int referenceOccasionId)
    {
        PhoenixRegistersMiscellaneousAppraisalOccasion.InsertMiscellaneousAppraisalOccasion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , occassion                                                                        
                                                                        , signoff
                                                                        , category
                                                                        , activeyn
                                                                        , referenceOccasionId
                                                                 );
    }

    private void UpdateAppraisalOccasion(int occassionid, string occassion, int signoff, int category, int activeyn,int referenceOccasionId)
    {
        PhoenixRegistersMiscellaneousAppraisalOccasion.UpdateMiscellaneousAppraisalOccasion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , occassionid
                                                                        , occassion                                                                        
                                                                        , signoff
                                                                        , category
                                                                        , activeyn
                                                                        , referenceOccasionId
                                                                 );
    }

    private void DeletetAppraisalOccasion(int occassionid)
    {
        PhoenixRegistersMiscellaneousAppraisalOccasion.DeleteMiscellaneousAppraisalOccasion(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , occassionid
                                                                 );
    }
   
    protected void TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        dgMiscellaneousAppraisalOccasion.Rebind();
    }

    protected void dgMiscellaneousAppraisalOccasion_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAppraisalOccasion(((UserControlAppraisalOccasion)e.Item.FindControl("ucOccasionInsert")).SelectedOccassion,
                    ((UserControlHard)e.Item.FindControl("ucCategoryAdd")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertAppraisalOccasion(
                    ((UserControlAppraisalOccasion)e.Item.FindControl("ucOccasionInsert")).SelectedOccassion,
                    (((RadCheckBox)e.Item.FindControl("chkAppraisalSignoffAdd")).Checked==true) ? 1 : 0,
                    int.Parse(((UserControlHard)e.Item.FindControl("ucCategoryAdd")).SelectedHard)
                    , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked==true) ? 1 : 0
                    , int.Parse(((UserControlAppraisalOccasion)e.Item.FindControl("ucOccasionInsert")).SelectedOccassionId)
                    );
                BindData();
                dgMiscellaneousAppraisalOccasion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string occasionId = (e.Item as GridDataItem).GetDataKeyValue("FLDOCCASIONID").ToString();
                DeletetAppraisalOccasion(int.Parse(occasionId));
                BindData();
                dgMiscellaneousAppraisalOccasion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string occasionId = (e.Item as GridDataItem).GetDataKeyValue("FLDOCCASIONID").ToString();
                UpdateAppraisalOccasion(
                                 General.GetNullableInteger(occasionId).Value,
                                 ((RadLabel)e.Item.FindControl("lblOccasionEdit")).Text,
                                 (((RadCheckBox)e.Item.FindControl("chkAppraisalSignoffedit")).Checked==true) ? 1 : 0,
                                 int.Parse(((RadLabel)e.Item.FindControl("lblcategoryIdedit")).Text),
                                 (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0,
                                 int.Parse(((RadLabel)e.Item.FindControl("lblOccasionrefIdedit")).Text)
                                 );
                BindData();
                dgMiscellaneousAppraisalOccasion.Rebind();
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

    protected void dgMiscellaneousAppraisalOccasion_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlHard ucCategoryEdit = (UserControlHard)e.Item.FindControl("ucCategoryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCategoryEdit != null) ucCategoryEdit.SelectedHard = drv["FLDCATEGORY"].ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            UserControlAppraisalOccasion ucOccasionInsert = (UserControlAppraisalOccasion)e.Item.FindControl("ucOccasionInsert");
            ucOccasionInsert.bind();
        }
    }

    protected void dgMiscellaneousAppraisalOccasion_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgMiscellaneousAppraisalOccasion.CurrentPageIndex + 1;
        BindData();
    }
}

