/* jalali.js  Gregorian to Jalali and inverse date convertor
 * Copyright (C) 2001  Roozbeh Pournader <roozbeh@sharif.edu>
 * Copyright (C) 2001  Mohammad Toossi <mohammad@bamdad.org>
 * Copyright (C) 2003,2008  Behdad Esfahbod <js@behdad.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You can receive a copy of GNU Lesser General Public License at the
 * World Wide Web address <http://www.gnu.org/licenses/lgpl.html>.
 *
 * For licensing issues, contact The FarsiWeb Project Group,
 * Computing Center, Sharif University of Technology,
 * PO Box 11365-8515, Tehran, Iran, or contact us the
 * email address <FWPG@sharif.edu>.
 */

/* Changes:
 * 
 * 2008-Jul-32:
 *  Use a remainder() function to fix conversion of ancient dates
 *	(before 1600 gregorian).  Reported by Shamim Rezaei.
 *
 * 2003-Mar-29:
 *		Ported to javascript by Behdad Esfahbod
 *
 * 2001-Sep-21:
 *	Fixed a bug with "30 Esfand" dates, reported by Mahmoud Ghandi
 *
 * 2001-Sep-20:
 *	First LGPL release, with both sides of conversions
 */
 
g_days_in_month = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
j_days_in_month = new Array(31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29);
 
function div(a,b) {
  return Math.floor(a/b);
}

function remainder(a,b) {
  return a - div(a,b)*b;
}

function gregorian_to_jalali(g /* array containing year, month, day*/ )
{
	var gy, gm, gd;
	var jy, jm, jd;
	var g_day_no, j_day_no;
	var j_np;
 
	var i;

	gy = g[0]-1600;
	gm = g[1]-1;
	gd = g[2]-1;

	g_day_no = 365*gy+div((gy+3),4)-div((gy+99),100)+div((gy+399),400);
	
	for (i=0;i<gm;++i)
		g_day_no += g_days_in_month[i];
	  
	if (gm>1 && ((gy%4==0 && gy%100!=0) || (gy%400==0)))
		/* leap and after Feb */
		++g_day_no;
	g_day_no += gd;
 
	j_day_no = g_day_no-79;
 
	j_np = div(j_day_no, 12053);
	j_day_no = remainder (j_day_no, 12053);

	jy = 979+33*j_np+4*div(j_day_no,1461);
	j_day_no = remainder (j_day_no, 1461);
	

	if (j_day_no >= 366) {
		jy += div((j_day_no-1),365);
		j_day_no = remainder ((j_day_no-1), 365);
	}
 
	for (i = 0; i < 11 && j_day_no >= j_days_in_month[i]; ++i) {
		j_day_no -= j_days_in_month[i];
	}
	
	jm = i+1;
	jd = j_day_no+1;
	if (jy.toString().length == 1) jy = "0" + jy;
	if (jm.toString().length == 1) jm = "0" + jm;
	if (jd.toString().length == 1) jd = "0" + jd;
	return new Array(jy, jm, jd);
}

function jalali_to_gregorian(j /* array containing year, month, day*/ )
{
	var j = j.split("/");
	var gy, gm, gd;
	var jy, jm, jd;
	var g_day_no, j_day_no;
	var leap;

	var i;

	jy = j[0]-979;
	jm = j[1]-1;
	jd = j[2]-1;

	j_day_no = 365*jy + div(jy,33)*8 + div((remainder (jy, 33)+3),4);
	for (i=0; i < jm; ++i)
		j_day_no += j_days_in_month[i];

	j_day_no += jd;

	g_day_no = j_day_no+79;

	gy = 1600 + 400*div(g_day_no,146097); /* 146097 = 365*400 + 400/4 - 400/100 + 400/400 */
	g_day_no = remainder (g_day_no, 146097);

	leap = 1;
	if (g_day_no >= 36525) /* 36525 = 365*100 + 100/4 */
	{
		g_day_no--;
		gy += 100*div(g_day_no,36524); /* 36524 = 365*100 + 100/4 - 100/100 */
		g_day_no = remainder (g_day_no, 36524);
		
		if (g_day_no >= 365)
			g_day_no++;
		else
			leap = 0;
	}

	gy += 4*div(g_day_no,1461); /* 1461 = 365*4 + 4/4 */
	g_day_no = remainder (g_day_no, 1461);

	if (g_day_no >= 366) {
		leap = 0;

		g_day_no--;
		gy += div(g_day_no, 365);
		g_day_no = remainder (g_day_no, 365);
	}

	for (i = 0; g_day_no >= g_days_in_month[i] + (i == 1 && leap); i++)
		g_day_no -= g_days_in_month[i] + (i == 1 && leap);
	gm = i+1;
	gd = g_day_no+1;
	if (gy.toString().length == 1) gy = "0" + gy;
	if (gm.toString().length == 1) gm = "0" + gm;
	if (gd.toString().length == 1) gd = "0" + gd;
	return new Array(gy, gm, gd);
}

function sqlDateToJalali(dateTime) {
	var dateTime = dateTime.trim();
	var res = dateTime.split(" ");
	var date = res[0];
	var time = res[1];
	var dateArray = date.split("-");
	j = gregorian_to_jalali(new Array(
		dateArray[0],
		dateArray[1],
		dateArray[2]
	));
	return j[0]+"/"+j[1]+"/"+j[2];
}

function jalali_today() {
  Today = new Date();
  j = gregorian_to_jalali(new Array(
								  Today.getFullYear(),
								  Today.getMonth()+1,
								  Today.getDate()
								  ));
  return j[0]+"/"+j[1]+"/"+j[2];
}