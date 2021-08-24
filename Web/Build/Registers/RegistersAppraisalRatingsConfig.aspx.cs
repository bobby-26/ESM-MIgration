using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAppraisalRatingsConfig : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalRatingsConfig.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRatingsConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalRatingsConfig.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");        
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;            
        }
    }

    protected void ShowExcel()
    {
      
        string[] alColumns = { "FLDOCCASSIONNAME","FLDRANGEFROM", "FLDRANGETO", "FLDRANGENAME", "FLDSHORTCODE" };
        string[] alCaptions = {"Occasion", "Range From", "Range To", "Grade", "Code" };


        DataSet ds = PhoenixRegistersAppraisalConfig.AppraisalConfigList(null, General.GetNullableInteger(ddlOccassion.SelectedOccassionId));

        Response.AddHeader("Content-Disposition", "attachment; filename=AppraisalRatingsConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Appraisal Ratings Config</h3></td>");
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

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ddlOccassion.SelectedOccassionId = "";
            BindData();
            gvRatingsConfig.Rebind();
        }
    }

    private void BindData()
    {
        string[] alColumns = { "FLDOCCASSIONNAME", "FLDRANGEFROM", "FLDRANGETO", "FLDRANGENAME", "FLDSHORTCODE" };
        string[] alCaptions = { "Occasion", "Range From", "Range To", "Grade", "Code" };


        DataSet ds = PhoenixRegistersAppraisalConfig.AppraisalConfigList(null,General.GetNullableInteger(ddlOccassion.SelectedOccassionId));
        
        General.SetPrintOptions("gvRatingsConfig", "Appraisal Ratings Config", alCaptions, alColumns, ds);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRatingsConfig.DataSource = ds;            
        }
        else
        {
            gvRatingsConfig.DataSource = "";
        }
    }
  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvRatingsConfig.Rebind();
    }

    

    private bool IsValidData(string rangefrom, string rangeto, string name, string code)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableInteger(rangefrom)==null)
            ucError.ErrorMessage = "Range From is required.";

        if (General.GetNullableInteger(rangefrom) > 10 || General.GetNullableInteger(rangefrom) < 0)
            ucError.ErrorMessage = "Range From should be between 0 to 10";

        if (General.GetNullableInteger(rangeto) == null)
            ucError.ErrorMessage = "Range To is required.";

        if (General.GetNullableInteger(rangeto) > 10 || General.GetNullableInteger(rangeto) < 0)
            ucError.ErrorMessage = "Range To should be between 0 to 10";


        if (General.GetNullableString(name) == null)
            ucError.ErrorMessage = "Grade is required.";

        if (General.GetNullableString(code) == null)
            ucError.ErrorMessage = "Code is required";

        return (!ucError.IsError);
    }

  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    
    protected void gvRatingsConfig_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;
            
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(
                            ((UserControlNumber)e.Item.FindControl("ucRangeFromAdd")).Text
                            , ((UserControlNumber)e.Item.FindControl("ucRangeToAdd")).Text
                            , ((RadTextBox)e.Item.FindControl("txtRangeNameAdd")).Text.Trim()
                            , ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text.Trim()
                        ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAppraisalConfig.AppraisalConfigInsert(
                          int.Parse(((UserControlNumber)e.Item.FindControl("ucRangeFromAdd")).Text.TrimStart('_'))
                        , int.Parse(((UserControlNumber)e.Item.FindControl("ucRangeToAdd")).Text.TrimStart('_'))
                        , ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text.Trim()
                        , ((RadTextBox)e.Item.FindControl("txtRangeNameAdd")).Text.Trim()
                        , General.GetNullableInteger(ddlOccassion.SelectedOccassionId)
                        );
                BindData();
                gvRatingsConfig.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersAppraisalConfig.AppraisalConfigDelete(new Guid(((RadLabel)e.Item.FindControl("lblID")).Text));
                BindData();
                gvRatingsConfig.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(
                          ((UserControlNumber)e.Item.FindControl("ucRangeFrom")).Text
                          , ((UserControlNumber)e.Item.FindControl("ucRangeTo")).Text
                          , ((RadTextBox)e.Item.FindControl("txtRangeName")).Text.Trim()
                          , ((RadTextBox)e.Item.FindControl("txtShortName")).Text.Trim()
                      ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersAppraisalConfig.AppraisalConfigUpdate(
                            new Guid(((RadLabel)e.Item.FindControl("lblIDEdit")).Text)
                            , int.Parse(((UserControlNumber)e.Item.FindControl("ucRangeFrom")).Text.TrimStart('_'))
                            , int.Parse(((UserControlNumber)e.Item.FindControl("ucRangeTo")).Text.TrimStart('_'))
                            , ((RadTextBox)e.Item.FindControl("txtShortName")).Text.Trim()
                            , ((RadTextBox)e.Item.FindControl("txtRangeName")).Text.Trim()
                            , General.GetNullableInteger(ddlOccassion.SelectedOccassionId)
                            );

                BindData();
                gvRatingsConfig.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRatingsConfig_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRatingsConfig_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

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
        }
    }
}
