﻿namespace Domain.Exceptions;

public class InfrastructureException : Exception
{
    public InfrastructureException(string message) : base(message) { }
}