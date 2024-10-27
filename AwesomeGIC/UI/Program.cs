// See https://aka.ms/new-console-template for more information
using AwesomeGIC.Application.Interfaces;
using AwesomeGIC.Application;
using AwesomeGIC.UI;
using AwesomeGIC.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;

var context = new BankAccountContext();
var accountRepository = new BankAccountRepository(context);
var interestRepository = new InterestRuleRepository(context);
var transactionRepository = new TransactionRepository(context);

var interestService = new InterestService(interestRepository, accountRepository);
var transactionService = new TransactionService(accountRepository, interestService, transactionRepository);

var inputHandler = new UserInputHandler(transactionService, interestService);
inputHandler.Start();