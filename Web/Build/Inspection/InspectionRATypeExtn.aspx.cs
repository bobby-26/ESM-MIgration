using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRATypeExtn : PhoenixBasePage
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
                ViewState["CURRENTINDEX"] = 1;
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

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ucRAType.SelectedHard != PhoenixCommonRegisters.GetHardCode(1, 201, "RAP") && ucRAType.SelectedHard != PhoenixCommonRegisters.GetHardCode(1, 201, "RAD"))
        {
            gvRAType.Columns[1].Visible = false;
        }

        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentTypeExtn.RATypeSearch(General.GetNullableInteger(ucRAType.SelectedHard),
                gvRAType.CurrentPageIndex + 1,
                gvRAType.PageSize,
               ref iRowCount,
            ref iTotalPageCount);

        gvRAType.DataSource = ds;
        gvRAType.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRAType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidData(ucRAType.SelectedHard,
                            ((RadTextBox)e.Item.FindControl("txtRAItemAdd")).Text,
                            ((UserControlMaskNumber)e.Item.FindControl("ucSortingOrderAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    int i = PhoenixInspectionRiskAssessmentTypeExtn.RATypeInsert(General.GetNullableInteger(ucRAType.SelectedHard),
                          ((RadTextBox)e.Item.FindControl("txtRAItemAdd")).Text,
                          ((CheckBox)e.Item.FindControl("chkGenericAdd")).Checked ? 1 : 0,
                          ((CheckBox)e.Item.FindControl("chkCargoAdd")).Checked ? 1 : 0,
                          ((CheckBox)e.Item.FindControl("chkNavigationAdd")).Checked ? 1 : 0,
                          ((CheckBox)e.Item.FindControl("chkMachineryAdd")).Checked ? 1 : 0,
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                          General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text),
                          General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucSortingOrderAdd")).Text));
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    int i = PhoenixInspectionRiskAssessmentTypeExtn.RATypeDelete(
                                ((RadLabel)e.Item.FindControl("lblRAID")).Text,
                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                }
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidData(ucRAType.SelectedHard,
                        ((RadTextBox)e.Item.FindControl("txtRAItem")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucSortingOrderEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    int i = PhoenixInspectionRiskAssessmentTypeExtn.RATypeUpdate(((RadLabel)e.Item.FindControl("lblRAIDEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtRAItem")).Text,
                        ((CheckBox)e.Item.FindControl("chkGenericEdit")).Checked ? 1 : 0,
                        ((CheckBox)e.Item.FindControl("chkCargoEdit")).Checked ? 1 : 0,
                        ((CheckBox)e.Item.FindControl("chkNavigationEdit")).Checked ? 1 : 0,
                        ((CheckBox)e.Item.FindControl("chkMachineryEdit")).Checked ? 1 : 0,
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text),
                        General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucSortingOrderEdit")).Text));
                }
            }
            BindData();
            gvRAType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string RAType, string Item, string sortingorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (RAType.Trim().Equals("Dummy") || RAType.Trim().Equals(""))
            ucError.ErrorMessage = "Risk Assessment Type is required.";
        if (Item.Trim().Equals(""))
            ucError.ErrorMessage = "Item is required.";

        if (sortingorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sorting Order is required.";

        return (!ucError.IsError);
    }

    protected void gvRAType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
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
        }
    }

    protected void gvRAType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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