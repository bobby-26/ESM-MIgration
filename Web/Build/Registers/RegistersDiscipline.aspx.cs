using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDiscipline : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDiscipline.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDiscipline')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDiscipline.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersDiscipline.AccessRights = this.ViewState;
            MenuRegistersDiscipline.MenuList = toolbar.Show();
            //MenuRegistersDiscipline.SetTrigger(pnlDisciplineEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDiscipline.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDISCIPLINECODE", "FLDDISCIPLINENAME", "FLDLEVEL" };
        string[] alCaptions = { "Code", "Responsibility", "Level" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDiscipline.DisciplineSearch(txtDisciplineCode.Text
                                                        , txtSearch.Text
                                                        , sortexpression
                                                        , sortdirection
                                                        , 1
                                                        , iRowCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        General.ShowExcel("Responsibility", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void RegistersDiscipline_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvDiscipline.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        string[] alColumns = { "FLDDISCIPLINECODE", "FLDDISCIPLINENAME", "FLDLEVEL" };
        string[] alCaptions = { "Code", "Responsibility", "Level" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDiscipline.DisciplineSearch(txtDisciplineCode.Text
            , txtSearch.Text
            , sortexpression
            , sortdirection
            , int.Parse(ViewState["PAGENUMBER"].ToString())
            , gvDiscipline.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvDiscipline", "Responsibility", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvDiscipline.DataSource = ds;
            gvDiscipline.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDiscipline.DataSource = "";
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvDiscipline_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscipline.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDiscipline_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
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

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cd.CommandName))
                    cd.Visible = false;
            }
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                UserControlRank ddl = (UserControlRank)e.Item.FindControl("ddlRank");                             
                ddl.SelectedRank = drv["FLDRANKID"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDiscipline_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string disciplineId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDDISCIPLINEID"].ToString();

                PhoenixRegistersDiscipline.DeleteDiscipline(int.Parse(disciplineId));
                BindData();
                gvDiscipline.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeAdd")).Text;
                string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameAdd")).Text;
                string disciplineLevel = ((RadTextBox)e.Item.FindControl("txtLevelAdd")).Text;
                string rank = ((UserControlRank)e.Item.FindControl("ddlRankAdd")).SelectedRank;

                if (IsValidInput(disciplineCode, disciplineName, disciplineLevel))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixRegistersDiscipline.InsertDiscipline(disciplineCode, disciplineName, int.Parse(disciplineLevel), General.GetNullableInteger(rank));
                    ucStatus.Text = "Information Added";
                }

                BindData();
                gvDiscipline.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string disciplineId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDDISCIPLINEID"].ToString();
                string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
                string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
                string disciplineLevel = ((RadTextBox)e.Item.FindControl("txtLevelEdit")).Text;
                string rank = ((UserControlRank)e.Item.FindControl("ddlRank")).SelectedRank;
                if (IsValidInput(disciplineCode, disciplineName, disciplineLevel))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                else
                {
                    PhoenixRegistersDiscipline.UpdateDiscipline(int.Parse(disciplineId), disciplineCode, disciplineName, int.Parse(disciplineLevel), General.GetNullableInteger(rank));
                    ucStatus.Text = "Responsibility information updated";
                }
                BindData();
                gvDiscipline.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidInput(string disciplineCode, string disciplineName, string disciplineLevel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (disciplineCode == "")
            ucError.ErrorMessage = "Code is required";

        if (disciplineName == "")
            ucError.ErrorMessage = "Name is required";

        if (disciplineLevel == "")
            ucError.ErrorMessage = "Level is required";

        return (ucError.IsError);

    }

}
