using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using SouthNests.Phoenix.Export2XL;
using System.IO;
using Telerik.Web.UI;

public partial class OwnerBudgetExpenseReport : PhoenixBasePage
{
    string spanWordInnerHtml = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
        toolbar.AddButton("Technical", "TECHNICAL", ToolBarDirection.Right);
        toolbar.AddButton("Lub Oil", "LUBOIL", ToolBarDirection.Right);
        toolbar.AddButton("Crew Expense", "EXPENSE", ToolBarDirection.Right);
        toolbar.AddButton("Crew Wages", "CREWWAGE", ToolBarDirection.Right);
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
        toolbar.AddButton("Revisions", "REVISIONS", ToolBarDirection.Right);
        toolbar.AddButton("Proposals", "PROPOSALS", ToolBarDirection.Right);
        MenuClose.AccessRights = this.ViewState;
        MenuClose.MenuList = toolbar.Show();
        MenuClose.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarword = new PhoenixToolbar();
        toolbarword.AddButton("Convert to Excel", "Excel", ToolBarDirection.Right);
        toolbarword.AddButton("Convert to Word", "WORD", ToolBarDirection.Right);
        MenuWord.AccessRights = this.ViewState;
        MenuWord.MenuList = toolbarword.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["proposalid"] != null)
            {
                ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
            }
            if (Request.QueryString["revisionid"] != null)
            {
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
            }




            GetData();
        }
    }

    private void GetData()
    {
        DataSet ds = PhoenixOwnerBudget.OwnerBudgetExpenseReport(new Guid(ViewState["PROPOSALID"].ToString()));
        string span1InnerHtml = "";
        string span2InnerHtml = "";
        string span3InnerHtml = "";
        string span4InnerHtml = "";
        string span5InnerHtml = "";
        string span6InnerHtml = "";
        string span7InnerHtml = "";
        string span8InnerHtml = "";
        string span9InnerHtml = "";
        string span10InnerHtml = "";
        int? vesselid = null;


        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            span2InnerHtml = span2InnerHtml + @"<table width=""100%""><tr>
                                          <td align=""left"" width=""30%"" ><h3>Vessel : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString()) + @"</h3></td>
                                          <td align=""left"" width=""40%"" ><h3>Owner : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDOWNERNAME"].ToString()) + @"</h3></td>
                                          <td align=""left"" width=""30%""><h3>Flag : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDFLAGNAME"].ToString()) + @"</h3></td>            
                                          </tr>
                                          <tr>
                                          <td align=""left"" width=""30%"" ><h3>Vessel Type : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDVESSELTYPE"].ToString()) + @"</h3></td>
                                          <td align=""left"" width=""40%"" ><h3>Year Built : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDYEARBUILD"].ToString()) + @"</h3></td>
                                          <td align=""left"" width=""30%""><h3>Date : " + HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDREPORTDATE"].ToString()) + @"</h3></td>            
                                          </tr>      
                                          </table>";


        }


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            span1InnerHtml = span1InnerHtml + @"<table width=""100%"">";
            span1InnerHtml = span1InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"" width=""40%"" >Expense</td>
                                          <td align=""right"" width=""20%"">Amount Per Year</td>
                                          <td align=""right"" width=""20%"">Amount Per Month</td>
                                          <td align=""right"" width=""20%"">Amount Per Day</td>            
                                                    </tr>";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["FLDEXPENSE"].ToString() == "Manning Expenses" || dr["FLDEXPENSE"].ToString() == "Technical Cost"
                        || dr["FLDEXPENSE"].ToString() == "Other Expenses" || dr["FLDEXPENSE"].ToString() == "Total Operating Cost")
                {
                    span1InnerHtml = span1InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"" width=""40%"">" + HttpUtility.HtmlDecode(dr["FLDEXPENSE"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERYEAR"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMONTH"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERDAY"].ToString()) + @"</td>            
                                                    </tr>";

                }
                else
                {
                    span1InnerHtml = span1InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">
                                          <td align=""left"" width=""40%"">" + HttpUtility.HtmlDecode(dr["FLDEXPENSE"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERYEAR"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMONTH"].ToString()) + @"</td>
                                          <td align=""right"" width=""20%"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERDAY"].ToString()) + @"</td>            
                                                    </tr>";

                }

            }
            span1InnerHtml = span1InnerHtml + @"</table>";

            string htmltext = "";
            DataSet dsparticulars = PhoenixOwnerBudget.BudgetVesselParticularsEdit(new Guid(ViewState["PROPOSALID"].ToString()));
            foreach (DataRow drp in dsparticulars.Tables[0].Rows)
            {
                vesselid = General.GetNullableInteger(drp["FLDVESSELID"].ToString());

                htmltext = @"</br></br><table width=""100%"">
                           <tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold"">
                            <td>A. MANNING EXPENSES</td> </tr>
                            <tr>
                            <td>1. Total Complement " + drp["FLDTOTALCOMPLEMENT"] + @" Men with 100% " + drp["FLDNATIONALITYNAME"] + @" Nationality employed under ITF agreement</td></tr>
                            <tr><td> " + drp["FLDSOFFREQUIRED"] + @" Senior Officers, " + drp["FLDJOFFREQUIRED"] + @" Junior Officers, " + drp["FLDTRAREQUIRED"] + @" Trainees and " + drp["FLDRATREQUIRED"] + @" Ratings</td></tr>
                            <tr><td>The Contract of the entire complement has been considered basis contract and not continuous employment</td></tr>
                            <tr><td>The crew wages subject to revision as per the market wages rate prior vessel take over</td></tr>
                            <tr><td>The crew cages is not included overlap wages will be charge as per the actual during handing over/taking over</td></tr>
                            <tr><td>In some cases where a suitable Indian crew compliment is not available, we may use other nationality</td></tr>
                            
                            <tr>
                            <td>2. Port of rotation:  <b>" + drp["FLDPORTOFROTATIONNAME"] + @"</b></td></tr>
                            <tr><td>Duration of Contract: </td></tr>
                            <tr><td>   Sr.Officers>> " + drp["FLDSOFFCONTRACT"] + @" months      Jr.Officers>> " + drp["FLDJOFFCONTRACT"] + @" months </td></tr>
                            <tr><td>   Trainees>> " + drp["FLDTRACONTRACT"] + @" months      Ratings>> " + drp["FLDRATCONTRACT"] + @" months </td></tr>
                            <tr><td>Our officers and crew undergo ship specific training in our in house training centre (Samudra Institute of Maritime Studies) prior they join a ship
                                and they are continuously being sent for upgrading courses to keep up with STCW convention/ISM requirement and latest technologies and equipment etc. 
                            </td></tr>
                            <tr><td>3. Victualing cost has been budgeted basis the provision cost for current trading area Our budget basis USD " + drp["FLDVICTUALLINGCOST"] + @" man/day</td></tr>
                            <tr><td></td></tr>    
                            <tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold"">
                            <td>B.TECHNICAL COST </td></tr>
                            <tr><td>These figures (stores,spares and repair/maintenance) are estimates and serve as aguide only.</tr></td>
                            <tr><td>These figures are subjected to change after inspection of vessel.</tr></td>";

                if (General.GetNullableInteger(drp["FLDCHINAYARDYN"].ToString()) == 0)
                {
                    htmltext = htmltext + @"<tr><td>
                                    The standard of workmanship & the quality of work done by Shipyard is of good quality and as per reputable International Ship-      
                                    </td></tr>
                                    <tr><td>Building Quality Standard. Machinery onboard the vessel is in good order and from Internationally recognised reputed Makers.
                                    </td></tr>    
                                ";
                }
                htmltext = htmltext + @" <tr><td>
                                     Our estimated cost does not include for any extensive maintenance and /or re-coating of cargo holds or ballast tanks.
                                     </td></tr> ";
                if (General.GetNullableInteger(drp["FLDVESSELTYPE"].ToString()) == 22)
                {
                    htmltext = htmltext + @"<tr><td>
                                     All tank cleaning chemical & material is not included in the budget, which usually supplied by Charterers.     
                                    </td></tr>
                                ";
                }

                htmltext = htmltext + @" <tr><td>
                                      Any spare supplies for shortfalls at time of inspection or pre-take over will be taken as non-budgeted cost.
                                     </td></tr>
                                     <tr><td>4. Stores include all deck, engine and cabin requirements of consumable items and assuming the vessel to be equipped 
                                         with normal stores at time of delivery. In case the stores level is low when we take over the ship, We will add-on necessary stores/
                                         equipment to meet oil major requirements & to reach normal stores level under non-budgeted expenses.    
                                     </td></tr>
                                    <tr><td>
                                        5. Spares are for regular replacement of machinery items and it is assumed that standard spare gear as per class requirement is onboard and machinery is in good order.
                                    </td></tr>
                                    <tr><td>
                                        6. Lubricating oil expense is based on consumption on similar vessel at " + drp["FLDPORTOFROTATIONNAME"] + @" and on " + drp["FLDSAILINGDAYPERANNUM"] + @" sailing days 
                                        Please note that the lube oil prices are extremely volatile these days, hence we will amend our estimated lube oil cost based  on latest market price prior vessel delivery.
                                        This may change depending on the actual M/E Type detail & present consumption  
                                    </td></tr>
                                    <tr><td>
                                        7. Running repairs and maintenance expenses are based on our experience with vessel of similiar age/type.  We also assumed the vessel is up to date regarding all International rules, including USCG, IMO and SOLAS
                                    </td></tr>
                                    <tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold"">
                                    <td>C. MANAGEMENT FEE
                                    </td></tr>
                                    <tr><td>
                                        8. Includes all services offered by our company such as: 
                                    </td></tr>          
                                    <tr><td>
                                        Full TECHNICAL management, including dealing with Classification Society/ Maritime Authorities include periodical and  occasional reports on the Vessel about repairs and maintenance. 
                                    </td></tr>
                                    <tr><td>
                                         Handling of all matters relates to the vessel's technical management, excluding operation and commercial employment of the vessel.
                                    </td></tr>                        
                                    <tr><td>
                                         Superintendent's attendance thrice a year for a maximum total period of 12 days per year. Thereafter US$500 per day will be charged  for superintendent attendance.
                                    </td></tr>
                                    <tr><td>
                                         Accounting of the vessel's expenses as per our accounting system.
                                    </td></tr>    
                                    <tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold"">
                                    <td>D. MISCELLANEOUS
                                    </td></tr>
                                    <tr><td>
                                        9. Expenses include Master petty expenses, Ships/Shore communication expenses, own supervisory travel, etc.
                                    </td></tr>    
                                    <tr><td>
                                          The estimated cost is in  line with the actual average costs that we have onboard our other vessel and this does not include any 
                                            expenses  related to regular trade to USA, COFR Expenses, Registration & Annual Tonnage Tax, etc.     
                                    </td></tr>
                                    ";
                if (General.GetNullableInteger(drp["FLDVESSELINSPECTEDYN"].ToString()) == 0)
                {
                    htmltext = htmltext + @"<tr><td>
                                     10. Vetting inspection fee for 3 oil major Inspection per annum and include pre-vetting inspection due to stringent requirements by the oil majors.
                                    </td></tr>
                                ";
                }

                htmltext = htmltext + @"<tr style=""text-align: left; margin: 0px; font-size:14px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold"">
                                    <td>E. MARINE INSURANCE
                                    </td></tr>
                            ";
                if (General.GetNullableInteger(drp["FLDINSURANCEYN"].ToString()) == 0)
                {
                    htmltext = htmltext + @"<tr><td>
                                            11. Insurance is for P&I, H&M, FD&D, War Risks & Disbursements. For the sake of budgeting a desk quote has been used.   
                                    </td></tr>
                                ";
                }
                else
                {
                    htmltext = htmltext + @"<tr><td>
                                     11. The above is not included in our estimates.
                                    </td></tr>
                                ";
                }

                if (General.GetNullableInteger(drp["FLDNEWBUILDINGYN"].ToString()) == 0)
                {
                    htmltext = htmltext + @"<tr><td>
                                           Note: - Our budget does not include any -pre-delivery expenses.
                                    </td></tr>
                                ";
                }
                else
                {
                    htmltext = htmltext + @"<tr><td>
                                     Note: - Our budget does not include any pre-takeover expenses, details of which shall be made available after our pre-takeover inspection.
                                    </td></tr>
                                ";
                }

                htmltext = htmltext + @"</table>";
            }
            span1InnerHtml = span1InnerHtml + htmltext;
            span1InnerHtml = span1InnerHtml + @"<br style=""page-break-before:always;"" />";
        }
        DataSet dsCrewWages = PhoenixOwnerBudget.OwnerBudgetVessel(new Guid(ViewState["PROPOSALID"].ToString()));
        if (dsCrewWages.Tables[0].Rows.Count > 0)
        {
            if (vesselid != null)
            {
                span3InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Crew Wages</h2></th></tr></table>
                                <table width=""100%"">";
                span3InnerHtml = span3InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"">Crew</td>
                                          <td align=""left"">Owner Scale</td>
                                          <td align=""left"">Current Wages Per Month Per Person</td>
                                          <td align=""left"">Proposed Wages Per Month Per Person</td>
                                          <td align=""left"">Total Current Wages Per Year</td>
                                          <td align=""left"">Total Current Wages Per Month</td>
                                          
                                          <td align=""left"">Total Proposed Wages Per Year</td>
                                          <td align=""left"">Total Proposed Wages Per Month</td>
                                          
                                                    </tr>";
                foreach (DataRow dr in dsCrewWages.Tables[0].Rows)
                {
                    string starttag = "";
                    if (General.GetNullableGuid(dr["FLDDTKEY"].ToString()) == null)
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                    }
                    else
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                    span3InnerHtml = span3InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDGROUPRANK"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOWNERSCALE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCURRENTWAGE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDWAGE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALCURRENTWAGEPERYEAR"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALCURRENTWAGE"].ToString()) + @"</td>
                                          
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALPROPOSEDWAGEPERYEAR"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALPROPOSEDWAGE"].ToString()) + @"</td>
                                          
                                                    </tr>";



                }
                span3InnerHtml = span3InnerHtml + @"</table>";
            }
            else
            {
                span3InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Crew Wages</h2></th></tr></table>
                                <table width=""100%"">";
                span3InnerHtml = span3InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"">Crew</td>
                                          <td align=""left"">Owner Scale</td>
                                          <td align=""left"">Proposed Wages Per Month Per Person</td>
                                          <td align=""left"">Total Proposed Wages Per Year</td>
                                          <td align=""left"">Total Proposed Wages Per Month</td>
                                          
                                                    </tr>";
                foreach (DataRow dr in dsCrewWages.Tables[0].Rows)
                {
                    string starttag = "";
                    if (General.GetNullableGuid(dr["FLDDTKEY"].ToString()) == null)
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                    }
                    else
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                    span3InnerHtml = span3InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDGROUPRANK"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOWNERSCALE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDWAGE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALPROPOSEDWAGEPERYEAR"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDTOTALPROPOSEDWAGE"].ToString()) + @"</td>
                                          
                                                    </tr>";



                }
                span3InnerHtml = span3InnerHtml + @"</table>";
            }

        }
        if (dsCrewWages.Tables[1].Rows.Count > 0)
        {
            span4InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Allowances</h2></th></tr></table>
                                <table width=""100%"">";
            span4InnerHtml = span4InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"">Allowance</td>
                                          <td align=""left""></td>
                                          <td align=""left""></td>
                                          <td align=""left""></td>
                                          <td align=""left""></td>
                                          <td align=""left""></td>
                                          <td align=""left""></td>
                                          <td align=""left"">Total Proposed Cost Per Year</td>
                                          <td align=""left"">Total Proposed Cost Per Month</td>
                                          
                                               </tr>";
            foreach (DataRow dr in dsCrewWages.Tables[1].Rows)
            {
                string starttag = "";
                if (dr["FLDHARDNAME"].ToString() == "Total")
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                span4InnerHtml = span4InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDHARDNAME"].ToString()) + @"</td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDAMOUNTPERYEAR"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDAMOUNTPERMONTH"].ToString()) + @"</td>
                                          
                                                    </tr>";



            }
            foreach (DataRow dr2 in dsCrewWages.Tables[2].Rows)
            {
                string starttag = "";
                starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                span4InnerHtml = span4InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr2["FLDHARDNAME"].ToString()) + @"</td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre""></td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr2["FLDPROPOSEDAMOUNTPERYEAR"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr2["FLDPROPOSEDAMOUNTPERMONTH"].ToString()) + @"</td>
                                                    </tr>";



            }
            span4InnerHtml = span4InnerHtml + @"</table>";

        }
        DataSet dscrewexpenses = PhoenixOwnerBudget.OwnerBudgetExpenseEdit(new Guid(ViewState["PROPOSALID"].ToString()), 112);
        if (dscrewexpenses.Tables[0].Rows.Count > 0)
        {
            span5InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Crew Expenses</h2></th></tr></table>
                                <table width=""100%"">";
            //            span5InnerHtml = span5InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
            //                                          <td align=""left"">Expense</td>
            //                                          <td align=""left"">Level</td>
            //                                          <td align=""left"">Contract Period</td>
            //                                          <td align=""left"">No. of Crew</td>
            //                                          <td align=""left"">Amount Per Man</td>
            //                                          <td align=""left"">Frequency</td>
            //                                          <td align=""left"">Amount Per Year</td>
            //                                          <td align=""left"">Amount Per Month</td>
            //                                          <td align=""left"">Amount Per Day</td>                               
            //                                                    </tr>";


            string category = "";
            foreach (DataRow dr in dscrewexpenses.Tables[0].Rows)
            {
                string starttag = "";

                if (General.GetNullableGuid(dr["FLDDTKEY"].ToString()) == null)
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                if (dr["FLDCATEGORY"] != null && (dr["FLDCATEGORY"].ToString() == category || dr["FLDCATEGORY"].ToString() == "Sub Total" || dr["FLDCATEGORY"].ToString() == "Grand Total"))
                {
                    span5InnerHtml = span5InnerHtml + starttag + @"
                                            <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> X 12/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                                            </tr>";
                }
                else
                {
                    span5InnerHtml = span5InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                <td align=""centre"" colspan = ""6"">" + HttpUtility.HtmlDecode(dr["FLDCATEGORY"].ToString()) + @" - USD " + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @" / Man</td></tr>
                                <tr>
                                <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> X 12/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                                </tr>";

                }

                //                span5InnerHtml = span5InnerHtml + starttag +@"
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCATEGORY"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDFREQUENCY"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMONTH"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERDAY"].ToString()) + @"</td>
                //                                                    </tr>";

                category = dr["FLDCATEGORY"].ToString();

            }
            span5InnerHtml = span5InnerHtml + @"</table>";

        }
        DataSet dstravelexpenses = PhoenixOwnerBudgetTravelCost.OwnerBudgetTravelCostList(new Guid(ViewState["PROPOSALID"].ToString()));
        if (dstravelexpenses.Tables[0].Rows.Count > 0)
        {
            span6InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Travel Expenses</h2></th></tr></table>
                                <table width=""100%"">";
            span6InnerHtml = span6InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"" colspan = ""6"" >Airfare to " + dstravelexpenses.Tables[0].Rows[0]["FLDPORTOFROTATION"] + @" -USD " + dstravelexpenses.Tables[0].Rows[0]["FLDAIRFAREPERMAN"] + @"/ Man  </td>
                                           </tr>";
            foreach (DataRow dr in dstravelexpenses.Tables[0].Rows)
            {
                string starttag = "";
                if (dr["FLDPORTOFROTATION"].ToString() == "Total")
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                span6InnerHtml = span6InnerHtml + starttag + @"
                                          <td align=""centre"">USDOLLARS</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAIRFAREPERMAN"].ToString()) + @"</td>  
                                          <td align=""centre""> X 1/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>  
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>    
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>    
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMONTH"].ToString()) + @"</td>        
                                                    </tr>";



            }
            span6InnerHtml = span6InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"" colspan = ""6"" >Expenses at " + dstravelexpenses.Tables[0].Rows[0]["FLDPORTOFROTATION"] + @" Such as transportation agency costs etc., estimated at USD " + dstravelexpenses.Tables[0].Rows[0]["FLDAGENCYFEEPERMAN"] + @" / man each</td>
                                           </tr>";
            foreach (DataRow dr in dstravelexpenses.Tables[0].Rows)
            {
                string starttag = "";
                if (dr["FLDPORTOFROTATION"].ToString() == "Total")
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                span6InnerHtml = span6InnerHtml + starttag + @"
                                          <td align=""centre"">USDOLLARS</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAGENCYFEEPERMAN"].ToString()) + @"</td>  
                                          <td align=""centre""> X 1/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>  
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>    
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>    
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAGENCYFEEPERMONTH"].ToString()) + @"</td>        
                                                    </tr>";



            }



            span6InnerHtml = span6InnerHtml + @"</table>";

        }
        DataSet dsotherexpenses = PhoenixOwnerBudget.OwnerBudgetExpenseEdit(new Guid(ViewState["PROPOSALID"].ToString()), 113);
        if (dsotherexpenses.Tables[0].Rows.Count > 0)
        {
            span7InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Other Crew Expenses</h2></th></tr></table>
                                <table width=""100%"">";
            //            span7InnerHtml = span7InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
            //                                          <td align=""left"">Expense</td>
            //                                          <td align=""left"">Level</td>
            //                                          <td align=""left"">Contract Period</td>
            //                                          <td align=""left"">No. of Crew</td>
            //                                          <td align=""left"">Amount Per Man</td>
            //                                          <td align=""left"">Frequency</td>
            //                                          <td align=""left"">Amount Per Year</td>
            //                                          <td align=""left"">Amount Per Month</td>
            //                                          <td align=""left"">Amount Per Day</td>
            //                                                    </tr>";

            string category = "";
            foreach (DataRow dr in dsotherexpenses.Tables[0].Rows)
            {
                string starttag = "";

                if (General.GetNullableGuid(dr["FLDDTKEY"].ToString()) == null)
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                if (dr["FLDCATEGORY"] != null && (dr["FLDCATEGORY"].ToString() == category || dr["FLDCATEGORY"].ToString() == "Sub Total" || dr["FLDCATEGORY"].ToString() == "Grand Total"))
                {
                    span7InnerHtml = span7InnerHtml + starttag + @"
                                            <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> X 12/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                                            </tr>";
                }
                else
                {
                    span7InnerHtml = span7InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                <td align=""centre"" colspan = ""6"">" + HttpUtility.HtmlDecode(dr["FLDCATEGORY"].ToString()) + @" - USD " + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @" / Man</td></tr>
                                <tr>
                                <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> X 12/" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @" X</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                                </tr>";

                }

                //                span5InnerHtml = span5InnerHtml + starttag +@"
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCATEGORY"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDOFFICERTYPE"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCONTRACTPERIOD"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDFREQUENCY"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNT"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMONTH"].ToString()) + @"</td>
                //                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERDAY"].ToString()) + @"</td>
                //                                                    </tr>";

                category = dr["FLDCATEGORY"].ToString();
            }
            span7InnerHtml = span7InnerHtml + @"</table>";

        }
        DataSet dsflagexpenses = PhoenixOwnerBudget.OwnerBudgetOtherExpenses(new Guid(ViewState["PROPOSALID"].ToString()));
        if (dsflagexpenses.Tables[0].Rows.Count > 0)
        {
            span8InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Flag Expenses</h2></th></tr></table>
                                <table width=""100%"">";

            string category = "";
            foreach (DataRow dr in dsflagexpenses.Tables[0].Rows)
            {
                string starttag = "";
                if (General.GetNullableString(dr["FLDLEVEL"].ToString()) == null)
                {
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                }
                else
                    starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                if (dr["FLDEXPENSES"] != null && (dr["FLDEXPENSES"].ToString() == category || dr["FLDEXPENSES"].ToString() == "Sub Total" || dr["FLDEXPENSES"].ToString() == "Grand Total"))
                {
                    span8InnerHtml = span8InnerHtml + starttag + @"
                                            <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> 12 X </td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDLEVEL"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERYEAR"].ToString()) + @"</td>
                                            </tr>";
                }
                else
                {
                    span8InnerHtml = span8InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                <td align=""centre"" colspan = ""6"">" + HttpUtility.HtmlDecode(dr["FLDEXPENSES"].ToString()) + @" - USD " + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @" / Man /Month</td></tr>
                                <tr>
                                <td align=""centre"">USDOLLARS</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERMAN"].ToString()) + @"</td>
                                <td align=""centre""> 12 X </td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDMANREQUIRED"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDLEVEL"].ToString()) + @"</td>
                                <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDAMOUNTPERYEAR"].ToString()) + @"</td>
                                </tr>";

                }
                category = dr["FLDEXPENSES"].ToString();


            }
            span8InnerHtml = span8InnerHtml + @"</table>";

        }
        DataSet dstechnicalexpenses = PhoenixOwnerBudget.OwnerBudgetTechnicalProposalEdit(new Guid(ViewState["PROPOSALID"].ToString()));
        if (dstechnicalexpenses.Tables[0].Rows.Count > 0)
        {
            if (vesselid != null)
            {
                span9InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Technical Expenses</h2></th></tr></table>
                                <table width=""100%"">";
                span9InnerHtml = span9InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"">Budget Group</td>
                                          <td align=""left"">Budget Code</td>
                                          <td align=""left"">Description</td>
                                          <td align=""left"">Current Budget Per Year</td>
                                          <td align=""left"">Current Budget Per Month</td>
                                          
                                          <td align=""left"">Proposed Budget Per Year</td>
                                          <td align=""left"">Proposed Budget Per Month</td>
                                          
                                          <td align=""left"">Remarks</td>                                     
                                                    </tr>";
                foreach (DataRow dr in dstechnicalexpenses.Tables[0].Rows)
                {
                    string starttag = "";
                    if (General.GetNullableGuid(dr["FLDPROPOSALID"].ToString()) == null)
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                    }
                    else
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                    span9InnerHtml = span9InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDBUDGETGROUP"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDBUDGETCODE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDDESCRIPTION"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCURRENTBUDGET"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDCURRENTBUDGETMONTHLY"].ToString()) + @"</td>
                                          
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDBUDGET"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDBUDGETMONTHLY"].ToString()) + @"</td>
                                          
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDREMARKS"].ToString()) + @"</td>
                                                    </tr>";



                }
                span9InnerHtml = span9InnerHtml + @"</table>";
            }
            else
            {
                span9InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Technical Expenses</h2></th></tr></table>
                                <table width=""100%"">";
                span9InnerHtml = span9InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                          <td align=""left"">Budget Group</td>
                                          <td align=""left"">Budget Code</td>
                                          <td align=""left"">Description</td>
                                          <td align=""left"">Proposed Budget Per Year</td>
                                          <td align=""left"">Proposed Budget Per Month</td>
                                          <td align=""left"">Remarks</td>                                     
                                                    </tr>";
                foreach (DataRow dr in dstechnicalexpenses.Tables[0].Rows)
                {
                    string starttag = "";
                    if (General.GetNullableGuid(dr["FLDPROPOSALID"].ToString()) == null)
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                    }
                    else
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                    span9InnerHtml = span9InnerHtml + starttag + @"
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDBUDGETGROUP"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDBUDGETCODE"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDDESCRIPTION"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDBUDGET"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDBUDGETMONTHLY"].ToString()) + @"</td>
                                          <td align=""centre"">" + HttpUtility.HtmlDecode(dr["FLDREMARKS"].ToString()) + @"</td>
                                                    </tr>";



                }
                span9InnerHtml = span9InnerHtml + @"</table>";
            }

        }

        DataTable dtLuboilEdit = PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilEdit(new Guid(ViewState["PROPOSALID"].ToString()));
        if (dtLuboilEdit.Rows.Count > 0)
        {
            span10InnerHtml = @"<table width = ""100%""><tr><th align=""centre""><h2>Lub Oil Expenses</h2></th></tr></table>
                                <table width=""100%"">";
            span10InnerHtml = span10InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                                    <td align=""left"">Lub Oil Stem Port</td>
                                                    <td align=""left"">" + dtLuboilEdit.Rows[0]["FLDPORTOFROTATION"].ToString() + @"</td></tr>
                                          <tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                            <td align=""left"">Lub Oil Supplier</td>
                                            <td align=""left"">" + dtLuboilEdit.Rows[0]["FLDNAME"].ToString() + @"</td>
                                          </tr>
                                          <tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                            <td align=""left"">Sailing Days</td>
                                            <td align=""left"">" + dtLuboilEdit.Rows[0]["FLDSAILINGDAYSPERYEAR"].ToString() + @"</td>
                                          </tr>
                                        </table>";
            ViewState["LUBOILID"] = General.GetNullableGuid(dtLuboilEdit.Rows[0]["FLDLUBOILID"].ToString());

        }
        if (General.GetNullableGuid(ViewState["LUBOILID"].ToString()) != null)
        {
            DataTable dtLubOil = PhoenixOwnerBudgetLubOil.OwnerBudgetLubOilLineItemList(new Guid(ViewState["LUBOILID"].ToString()));
            if (dtLubOil.Rows.Count > 0)
            {
                span10InnerHtml = span10InnerHtml + @"<table width = ""100%"">
                                                        <tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">
                                                            <td colspan=""4"">Lub Oil Expenses /mth</td>
                                                        </tr>";
                foreach (DataRow dr in dtLubOil.Rows)
                {
                    string starttag = "";
                    if (General.GetNullableGuid(dr["FLDPROPOSALID"].ToString()) == null)
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif; font-weight:bold;"">";
                        span10InnerHtml = span10InnerHtml + starttag + @"<td></td>
                                                                   <td>Total USD</td>
                                                                   <td>&nbsp&nbsp = &nbsp&nbsp " + HttpUtility.HtmlDecode(dr["FLDPRICEPERMONTH"].ToString()) + @"</td></tr>";
                    }
                    else if (dr["FLDPRODUCTNAME"].ToString() != "Other grades")
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                        span10InnerHtml = span10InnerHtml + starttag + @"<td>" + HttpUtility.HtmlDecode(dr["FLDPRODUCTNAME"].ToString()) + @"</td>
                                                                   <td>(" + HttpUtility.HtmlDecode(dr["FLDOILPERDAY"].ToString()) + @" x " + HttpUtility.HtmlDecode(dr["FLDSAILINGDAYSPERYEAR"].ToString()) + @" x " + HttpUtility.HtmlDecode(dr["FLDUNITPRICE"].ToString()) + @")/12</td>
                                                                   <td>&nbsp&nbsp = &nbsp&nbsp " + HttpUtility.HtmlDecode(dr["FLDPRICEPERMONTH"].ToString()) + @"</td></tr>";
                    }
                    else
                    {
                        starttag = @"<tr style=""text-align: left; margin: 0px; font-size:11px; font-family: Tahoma, Arial, Helvetica, Sans-serif;"">";

                        span10InnerHtml = span10InnerHtml + starttag + @"<td>" + HttpUtility.HtmlDecode(dr["FLDPRODUCTNAME"].ToString()) + @"</td>
                                                                   <td>" + HttpUtility.HtmlDecode(dr["FLDPRICEPERYEAR"].ToString()) + @"/12</td>
                                                                   <td>&nbsp&nbsp = &nbsp&nbsp " + HttpUtility.HtmlDecode(dr["FLDPRICEPERMONTH"].ToString()) + @"</td></tr>";
                    }


                }
                span10InnerHtml = span10InnerHtml + @"</table>";

            }
        }

        spanWordInnerHtml = @"<table><tr><td align=""center""><h1>Proposed Operating Cost</h1></td></tr><tr><td>"
                            + span2InnerHtml + @"</td></tr><tr><td>"
                            + span1InnerHtml + @"</td></tr><tr><td>"
                            + span3InnerHtml + @"</td></tr><tr><td>"
                            + span4InnerHtml + @"</td></tr><tr><td>"
                            + span5InnerHtml + @"</td></tr><tr><td>"
                            + span6InnerHtml + @"</td></tr><tr><td>"
                            + span7InnerHtml + @"</td></tr><tr><td>"
                            + span8InnerHtml + @"</td></tr><tr><td>"
                            + span9InnerHtml + @"</td></tr><tr><td>"
                            + span10InnerHtml + @"</td></tr></table>";

        span1.InnerHtml = spanWordInnerHtml;

    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PROPOSALS"))
            {
                Response.Redirect("OwnerBudgetProposal.aspx");
            }
            if (CommandName.ToUpper().Equals("REVISIONS"))
            {
                Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
            }
            if (CommandName.ToUpper().Equals("CREWWAGE"))
            {
                Response.Redirect("../OwnerBudget/OwnerBudgetProposedCrewWages.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
            }
            if (CommandName.ToUpper().Equals("TECHNICAL"))
            {
                Response.Redirect("../OwnerBudget/OwnerBudgetTechnicalProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
            }
            if (CommandName.ToUpper().Equals("EXPENSE"))
            {
                Response.Redirect("../OwnerBudget/OwnerBudgetCrewExpense.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
            }
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../OwnerBudget/OwnerBudgetVesselParticulars.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
            }
            if (CommandName.ToUpper().Equals("LUBOIL"))
            {
                Response.Redirect("../OwnerBudget/OwnerBudgetLubOil.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuFind_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
    }
    protected void MenuWord_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("WORD"))
            {
                //string strHtml = spanWordInnerHtml;

                //StringWriter sw = new StringWriter();
                //HtmlTextWriter w = new HtmlTextWriter(sw);
                //spanWord.RenderControl(w);
                //string strHtml = sw.GetStringBuilder().ToString();
                //PhoenixOwnerBudget.ConvertToWord(strHtml);

                Response.AddHeader("Content-Disposition", "attachment; filename=OperatingBudgets.doc");
                Response.ContentType = "application/vnd.msword";
                Response.Write(spanWordInnerHtml);
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                PhoenixOwnerBudget2XL.Export2XLOwnerBudgetProposal(new Guid(ViewState["PROPOSALID"].ToString()));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
