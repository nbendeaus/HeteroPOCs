using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeteroPOCs.Controllers
{
    public class CalenderController : Controller
    {
        // GET: Calender
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Generates an iCalendar .ics link and returns it to the user.
        /// </summary>
        /// <param name="downloadFileName">Name of the download file to return.</param>
        /// <param name="evt">The Event.</param>
        /// <returns>The .ics file.</returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult AddToICalendar(string downloadFileName, int eventId)
        {
            // replace db with however you call your Entity Framework or however you get your data.
            // In this example, we have an Events collection in our model.
            using (
                var db =
                    new ExampleEntities(
                        ConfigurationManager.ConnectionStrings[YourConnectionString].ConnectionString))
            {
                // Alternatively, you may use db.Events.Find(eventId) if this fits better.
                var demoEvent = db.Events.Single(getEvent => getEvent.ID == eventId);

                var icalStringbuilder = new StringBuilder();

                icalStringbuilder.AppendLine("BEGIN:VCALENDAR");
                icalStringbuilder.AppendLine("PRODID:-//MyTestProject//EN");
                icalStringbuilder.AppendLine("VERSION:2.0");

                icalStringbuilder.AppendLine("BEGIN:VEVENT");
                icalStringbuilder.AppendLine("SUMMARY;LANGUAGE=en-us:" + demoEvent.EventName);
                icalStringbuilder.AppendLine("CLASS:PUBLIC");
                icalStringbuilder.AppendLine(string.Format("CREATED:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                icalStringbuilder.AppendLine("DESCRIPTION:" + demoEvent.Description);
                icalStringbuilder.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", demoEvent.StartDateTime));
                icalStringbuilder.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", demoEvent.EndDateTime));
                icalStringbuilder.AppendLine("SEQUENCE:0");
                icalStringbuilder.AppendLine("UID:" + Guid.NewGuid());
                icalStringbuilder.AppendLine(
                    string.Format(
                        "LOCATION:{0}\\, {1}\\, {2}\\, {3} {4}",
                        evt.LocationName,
                        evt.Address,
                        evt.City,
                        evt.State,
                        evt.ZipCode).Trim());
                icalStringbuilder.AppendLine("END:VEVENT");
                icalStringbuilder.AppendLine("END:VCALENDAR");

                var bytes = Encoding.UTF8.GetBytes(icalStringbuilder.ToString());

                return this.File(bytes, "text/calendar", downloadFileName);
            }
        }






    }
}