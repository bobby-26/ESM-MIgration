using System;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewTravelHopListLocalArrangements : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CITYNAME"] = null;
                ViewState["VESSELID"] = null;
                ViewState["CITYID"] = null;
                gvHopList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','Travel Request Filter','" + Session["sitepath"] + "/Crew/CrewTravelHopListFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelHopListLocalArrangements.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuHopList.AccessRights = this.ViewState;
            MenuHopList.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuHotListLA.AccessRights = this.ViewState;
            MenuHotListLA.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuHopList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvHopList.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentTravelHopListFilter = null;
                BindData();
                gvHopList.Rebind();
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
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            NameValueCollection nvc = Filter.CurrentTravelHopListFilter;

            DataSet ds = new DataSet();

            ds = PhoenixCrewTravelLocalArrangements.CrewTravelLocalArrangementsSearch(
                   General.GetNullableInteger(nvc != null ? nvc.Get("txtArrivalCity") : string.Empty)
                 , General.GetNullableInteger(nvc != null ? nvc.Get("ucVessel") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("txtArrivalDateFrom") : string.Empty)
                 , General.GetNullableDateTime(nvc != null ? nvc.Get("txtArrivalDateTo") : string.Empty)
                  , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
               gvHopList.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["CITYID"] = ds.Tables[0].Rows[0]["FLDDESTINATIONID"].ToString();
                gvHopList.DataSource = ds;
                gvHopList.VirtualItemCount = iRowCount;
            }
            else
            {
                gvHopList.DataSource = "";
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
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();        
        gvHopList.Rebind();
    }

    protected void gvHopList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SAVE")
            {
                RadLabel lblHopLineItemId = (RadLabel)e.Item.FindControl("lblHopLineItemId");
                RadListBox cblArrangements = (RadListBox)e.Item.FindControl("cblArrangements");
                if (lblHopLineItemId != null && cblArrangements != null)
                {
                    StringBuilder strArrangementsList = new StringBuilder();

                    foreach (RadListBoxItem item in cblArrangements.Items)
                    {
                        if (item.Checked == true)
                        {
                            strArrangementsList.Append(item.Value);
                            strArrangementsList.Append(",");
                        }
                    }
                    if (strArrangementsList.Length > 1)
                    {
                        strArrangementsList.Remove(strArrangementsList.Length - 1, 1);
                    }
                    PhoenixCrewTravelQuoteLine.CrewTravelLocalArrangementsUpadte(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblHopLineItemId.Text.ToString()), General.GetNullableString(strArrangementsList.ToString()));
                }

                BindData();
                gvHopList.Rebind();
                ucStatus.Text = "Information Saved";
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

    protected void gvHopList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHopList.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvHopList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblHotelBookedYN = (RadLabel)e.Item.FindControl("lblHotelBookedYN");
                RadLabel lblArrangements = (RadLabel)e.Item.FindControl("lblArrangements");

                RadListBox cblArrangements = (RadListBox)e.Item.FindControl("cblArrangements");

                if (cblArrangements != null)
                {
                    cblArrangements.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 216);
                    cblArrangements.DataBind();

                    if (lblArrangements != null && lblArrangements.Text != "")
                    {
                        string[] Arrangements = lblArrangements.Text.ToString().Split(',');
                        foreach (string s in Arrangements)
                        {
                            foreach (RadListBoxItem item in cblArrangements.Items)
                            {
                                if (int.Parse(s) == int.Parse(item.Value))
                                {
                                    item.Checked = true;

                                }
                            }
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
}