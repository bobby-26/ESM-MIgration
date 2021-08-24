using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewVesselPositionArrivalList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            {
                if (!IsPostBack)
                {
                    ViewState["PAGENUMBER"] = 1;
                    ViewState["SORTEXPRESSION"] = null;
                    ViewState["SORTDIRECTION"] = null;
                    ViewState["VESSELID"] = ucVessel.SelectedVessel;
                    gvVesselPositionArrival.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                }
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Arrival", "ARRIVAL");
            toolbarmain.AddButton("Noon Report", "NOONREPORT");
           



            MenuVesselPosition.AccessRights = this.ViewState;
            MenuVesselPosition.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbararrival = new PhoenixToolbar();
            toolbararrival.AddFontAwesomeButton("javascript:parent.openNewWindow('code1','','" + Session["sitepath"] + "/Crew/CrewVesselPositionArrival.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuVesselPositionArrival.AccessRights = this.ViewState;
            MenuVesselPositionArrival.MenuList = toolbararrival.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselPosition_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (ViewState["VESSELID"] == null)
                return;

            if (CommandName.ToUpper().Equals("ARRIVAL"))
            {
                ViewState["TABNAME"] = "ARRIVAL";
                Response.Redirect("../Crew/CrewVesselPositionArrivalList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("NOONREPORT"))
            {
                ViewState["TABNAME"] = "NOONREPORT";
                Response.Redirect("../Crew/CrewVesselPositionNoonReportList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), false);
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
        gvVesselPositionArrival.Rebind();
    }
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ucVessel.SelectedVessel;
       gvVesselPositionArrival.Rebind();
    }

    protected void BindData(object sender, EventArgs e)
    {
        gvVesselPositionArrival.Rebind(); ;

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            string month = ddlMonth.SelectedHard;
            string year = ddlYear.SelectedQuick;
            string voyageno = string.Empty;
            if (month.Trim() != string.Empty && month.Trim() != "Dummy" && year.Trim() != string.Empty && year.Trim() != "Dummy")
            {
                voyageno = ((RadComboBox)ddlMonth.FindControl("ddlHard")).SelectedItem.Text + ((RadComboBox)ddlYear.FindControl("ddlQuick")).SelectedItem.Text;
            }
            else
                voyageno = string.Empty;


            DataTable dt = PhoenixCrewVesselPosition.SearchVesselPosition(General.GetNullableInteger(ucVessel.SelectedVessel.ToString()), voyageno, sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], gvVesselPositionArrival.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount);
            gvVesselPositionArrival.DataSource = dt;
            gvVesselPositionArrival.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    

    private bool IsValidateVesselPosition(string vesselid, string month, string year, string portid, string eta, string etd, string etatime, string etdtime)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        etatime = etatime.Replace("AM", string.Empty).Replace("PM", string.Empty).Trim();
        etdtime = etdtime.Replace("AM", string.Empty).Replace("PM", string.Empty).Trim();
        //int result;
        DateTime resultDate;
        if (vesselid.Trim() == "Dummy" || vesselid.Trim() == "")
            ucError.ErrorMessage = "Vessel is required.";

        if (month.Trim() == "Dummy" || month.Trim() == "")
            ucError.ErrorMessage = "Month is required.";

        if (General.GetNullableInteger(year) == null)
            ucError.ErrorMessage = "Year is required.";

        if (portid.Trim() == "Dummy" || portid.Trim() == "")
            ucError.ErrorMessage = "Port is required.";
        //if (!string.IsNullOrEmpty(etatime) && int.TryParse(etatime.Substring(0, etatime.IndexOf(":")), out result) && result > 12)
        //{
        //    ucError.ErrorMessage = "Invalid ETA(Time).";
        //}
        //if (!string.IsNullOrEmpty(etatime) && int.TryParse(etatime.Substring(etatime.IndexOf(":") + 1), out result) && result > 59)
        //{
        //    ucError.ErrorMessage = "Invalid ETA(Time).";
        //}
        //if (!string.IsNullOrEmpty(etdtime) && int.TryParse(etdtime.Substring(0, etdtime.IndexOf(":")), out result) && result > 12)
        //{
        //    ucError.ErrorMessage = "Invalid ETA(Time).";
        //}
        //if (!string.IsNullOrEmpty(etdtime) && int.TryParse(etdtime.Substring(etdtime.IndexOf(":") + 1), out result) && result > 59)
        //{
        //    ucError.ErrorMessage = "Invalid ETA(Time).";
        //}
        if (!string.IsNullOrEmpty(eta) && !string.IsNullOrEmpty(etd) && DateTime.TryParse(eta, out resultDate)
               && DateTime.Compare(resultDate, DateTime.Parse(etd)) > 0)
        {
            ucError.ErrorMessage = "ETA should be less than ETD";
        }
        if (!string.IsNullOrEmpty(eta) && etatime==":")
            ucError.ErrorMessage = "ETA date and time required";
        if (!string.IsNullOrEmpty(etd) && etdtime == ":")
            ucError.ErrorMessage = "ETD date and time required";
        return (!ucError.IsError);
    }

    protected void gvVesselPositionArrival_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselPositionArrival.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselPositionArrival_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT") return;

        if (e.CommandName.ToString().ToUpper().Equals("SELECT"))
        {
            RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVesselId");
            ViewState["VESSELID"] = lblVessel.Text;
        }
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {
            try
            {
                string vessel = ViewState["VESSELID"].ToString();
                string month = ddlMonth.SelectedHard;
                string year = ddlYear.SelectedQuick;
                string port = ((UserControlSeaport)e.Item.FindControl("ddlPortAdd")).SelectedSeaport;
                string eta = ((UserControlDate)e.Item.FindControl("txtETAAdd")).Text;
                string etd = ((UserControlDate)e.Item.FindControl("txtETDAdd")).Text;
                string etatime = ((UserControlMaskedTextBox)e.Item.FindControl("txtETATimeAdd")).TextWithLiterals;
                string etdtime = ((UserControlMaskedTextBox)e.Item.FindControl("txtETDTimeAdd")).TextWithLiterals;
                etatime = etatime.Replace("AM", "").Replace("PM", "").Trim() == "__:__" ? string.Empty : etatime;
                etdtime = etdtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__" ? string.Empty : etdtime;


                if (!IsValidateVesselPosition(vessel, month, year, port, eta, etd, etatime, etdtime))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewVesselPosition.InsertVesselPosition(int.Parse(vessel)
                                                        , ((RadComboBox)ddlMonth.FindControl("ddlHard")).SelectedItem.Text + ((RadComboBox)ddlYear.FindControl("ddlQuick")).SelectedItem.Text
                                                        , int.Parse(port)
                                                        , General.GetNullableDateTime(eta + " " + etatime)
                                                        , General.GetNullableDateTime(etd + " " + etdtime)
                                                        );

                gvVesselPositionArrival.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToString().ToUpper() == "UPDATE")
        {
            try
            {
                string positionid = ((RadLabel)e.Item.FindControl("lblPositionIdEdit")).Text;
                string port = ((UserControlSeaport)e.Item.FindControl("ddlPortEdit")).SelectedSeaport;
                string eta = ((UserControlDate)e.Item.FindControl("txtETAEdit")).Text;
                string etd = ((UserControlDate)e.Item.FindControl("txtETDEdit")).Text;
                string etatime = ((UserControlMaskedTextBox)e.Item.FindControl("txtETATimeEdit")).TextWithLiterals;
                string etdtime = ((UserControlMaskedTextBox)e.Item.FindControl("txtETDTimeEdit")).TextWithLiterals;
                etatime = etatime.Replace("AM", "").Replace("PM", "").Trim() == "__:__" ? string.Empty : etatime;
                etdtime = etdtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__" ? string.Empty : etdtime;
                if (!IsValidateVesselPosition("something", "something", "1900", port, eta, etd, etatime, etdtime))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewVesselPosition.UpdateVesselPosition(int.Parse(positionid)
                                                        , int.Parse(port)
                                                        , General.GetNullableDateTime(eta + " " + etatime)
                                                        , General.GetNullableDateTime(etd + " " + etdtime)
                                                        );
               
                gvVesselPositionArrival.Rebind();
            }

            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

           
        }
        if (e.CommandName.ToUpper() == "DELETE")
        {
            try
            {

                string positionid = ((RadLabel)e.Item.FindControl("lblPositionId")).Text;
                PhoenixCrewVesselPosition.DeleteVesselPosition(int.Parse(positionid));


                gvVesselPositionArrival.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
    }

    protected void gvVesselPositionArrival_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            UserControlSeaport ucSeaPort = (UserControlSeaport)e.Item.FindControl("ddlPortEdit");
            DataRowView drvSeaPort = (DataRowView)e.Item.DataItem;
            if (ucSeaPort != null)
            {
                ucSeaPort.SelectedSeaport = drvSeaPort["FLDSEAPORTID"].ToString();
            }
        }
    }

    protected void gvVesselPositionArrival_SortCommand(object sender, GridSortCommandEventArgs e)
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
