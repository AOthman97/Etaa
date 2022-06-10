TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", projects.UserId));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                    