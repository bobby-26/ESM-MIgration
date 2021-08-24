using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersAppraisalOccasion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalOccasion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInstituteFaculty')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalOccasion.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersAppraisalOccasion.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");        
        MenuAppraisalOccasion.AccessRights = this.ViewState;
        MenuAppraisalOccasion.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAppraisalOccasion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
      
        string[] alColumns = { "FLDOCCASION", "FLDSHORTCODE" };
        string[] alCaptions = { "Occasion", "Short Code" };
               
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionSearch(null
                                                                                  , General.GetNullableString(txtOccasionSearch.Text.Trim())
                                                                                  , General.GetNullableString(txtShortCode.Text.Trim())
                                                                                  , sortexpression
                                                                                  , sortdirection
                                                                                  , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                  , gvAppraisalOccasion.PageSize
                                                                                  , ref iRowCount
                                                                                  , ref iTotalPageCount);

        General.SetPrintOptions("gvInstituteFaculty", "Occasion", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvAppraisalOccasion.DataSource = ds;
            gvAppraisalOccasion.VirtualItemCount = iRowCount;
        }
        else
        {
            gvAppraisalOccasion.DataSource = "";
        }
    }
    
    protected void MenuAppraisalOccasion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {              
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvAppraisalOccasion.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDOCCASION", "FLDSHORTCODE" };
                string[] alCaptions = { "Occasion", "Short Code" };

                string sortexpression;
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionSearch(null
                                                                                  , General.GetNullableString(txtOccasionSearch.Text.Trim())
                                                                                  , General.GetNullableString(txtShortCode.Text.Trim())
                                                                                  , sortexpression
                                                                                  , sortdirection
                                                                                  , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                  , gvAppraisalOccasion.PageSize
                                                                                  , ref iRowCount
                                                                                  , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Occasion", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {              
                txtOccasionSearch.Text = "";
                txtShortCode.Text = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();                
                gvAppraisalOccasion.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOccasion(string occasion,string shortcode)        
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(occasion))
            ucError.ErrorMessage = "occasion is required";
        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Short code is required";
        return (!ucError.IsError);
    }

    protected void gvAppraisalOccasion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string occasion = null, shortcode = null; int active;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                occasion = ((RadTextBox)e.Item.FindControl("txtOccasionInsert")).Text;
                shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeInsert")).Text;
                active = ((RadCheckBox)e.Item.FindControl("chkactiveadd")).Checked == true ? 1 : 0;

                if (!IsValidOccasion(occasion, shortcode))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionInsert(occasion.Trim()
                                                                                , shortcode.Trim()
                                                                                , active);

                BindData();
                gvAppraisalOccasion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                occasion = ((RadTextBox)e.Item.FindControl("txtoccasionedit")).Text;
                shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text;
                int activeyn = ((RadCheckBox)e.Item.FindControl("chkactive")).Checked == true ? 1 : 0;

                string occasionId = (e.Item as GridDataItem).GetDataKeyValue("FLDAPPRAISALOCCASIONID").ToString();
                if (!IsValidOccasion(occasion, shortcode))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionUpdate(occasion.Trim()
                                                                        , shortcode.Trim()
                                                                        , General.GetNullableInteger(occasionId).Value
                                                                        , activeyn
                                                                        );

                BindData();
                gvAppraisalOccasion.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string occasionId = (e.Item as GridDataItem).GetDataKeyValue("FLDAPPRAISALOCCASIONID").ToString();
                PhoenixRegistersAppraisalOccasion.RegistersAppraisalOccasionDelete(General.GetNullableInteger(occasionId).Value);

                BindData();
                gvAppraisalOccasion.Rebind();
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
            BindData();
        }
    }
    protected void gvAppraisalOccasion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAppraisalOccasion.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAppraisalOccasion_ItemDataBound(object sender, GridItemEventArgs e)
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
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton OccasionMapping = (LinkButton)e.Item.FindControl("cmdOwnerMapping");
            if (OccasionMapping != null)
            {
                OccasionMapping.Visible = SessionUtil.CanAccess(this.ViewState, OccasionMapping.CommandName);
                OccasionMapping.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Registers/RegistersAppraisalOwnerOccasionMapping.aspx?occasionid=" + drv["FLDAPPRAISALOCCASIONID"] + "'); return false;");
            }
        }
        if (e.Item is GridDataItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }
}
