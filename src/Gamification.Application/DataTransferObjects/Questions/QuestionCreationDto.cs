//-----------------------------------------------------------------------
// This file is autogenerated by EntityCore
// <auto-generated />
//-----------------------------------------------------------------------

using System;
using Gamification.Domain.Entities;
using System.Collections.Generic;

namespace DataTransferObjects.Questions;

public class QuestionCreationDto
{
	public string Text { get; set; }
	public int TestId { get; set; }
	public List<int> AnswersIds { get; set; }
}
